
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using DocumentService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddHttpClient<TemplateService>();
builder.Services.AddScoped<LatexService>();
builder.Services.AddScoped<FileService>();

// APP *******************
var app = builder.Build();

app.UseCors();
app.MapReverseProxy();
app.UseHttpsRedirection();

app.MapPost("/document/{id}/{ext}", async (
    [FromServices] ILogger<Program> logger,
    [FromServices] TemplateService templateService,
    [FromServices] LatexService latexService,
    [FromRoute] string id,
    [FromRoute] string ext,
    [FromBody] RequestModel request) =>
{
    var html = await templateService.GetHtml(id, request);

    logger.LogInformation(html);

    if (ext == "latex")
    {
        
    }

    var (pdfBytes, workDir) = await latexService.HtmlToLatexToPdf(request, html);

    if (ext == "pdf")
        return Results.File(pdfBytes, contentType: "application/pdf", fileDownloadName: $"{FileService.GetRandomFilenameWithoutExtension()}.pdf");

    var zipBytes = FileService.ZipDirectory(workDir);

    return Results.File(zipBytes, contentType: "application/zip", fileDownloadName: $"{FileService.GetRandomFilenameWithoutExtension()}.zip");

})
.Accepts<object>("application/json")
.Produces<byte[]>((int)HttpStatusCode.OK, "application/pdf")
.Produces<byte[]>((int)HttpStatusCode.OK, "application/zip");

app.Run();

public record RequestModel(DocumentData Document, object Template);

public record DocumentData(string Telephone, string Email);