
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

app.MapGet("/static/{filename}", async (
    [FromServices] FileService fileService,
    [FromRoute] string fileName) =>
{
    var fileBytes = await fileService.GetStaticFile(fileName);

    if (fileBytes is null) return Results.NotFound(fileName);

    return Results.File(fileBytes, contentType: "application/octet-stream", fileDownloadName: fileName);
})
.Produces<byte[]>((int)HttpStatusCode.OK, "application/octet-stream");

app.MapPost("/document/{id}/pdf", async (
    [FromServices] ILogger<Program> logger,
    [FromServices] TemplateService templateService,
    [FromServices] LatexService latexService,
    [FromRoute] string id,
    [FromBody] object request) =>
{
    var html = await templateService.GetHtml(id, request);

    logger.LogInformation(html);

    var (pdfBytes, workDir) = await latexService.HtmlToLatexToPdf(html);

    var success = FileService.DeleteDirectory(workDir);

    logger.LogInformation("workDir deleted: {Success}", success);

    return Results.File(pdfBytes, contentType: "application/pdf", fileDownloadName: $"{FileService.GetRandomFilenameWithoutExtension()}.pdf");
})
.Accepts<object>("application/json")
.Produces<byte[]>((int)HttpStatusCode.OK, "application/pdf");

app.MapPost("/document/{id}/zip", async (
    [FromServices] ILogger<Program> logger,
    [FromServices] TemplateService templateService,
    [FromServices] LatexService latexService,
    [FromRoute] string id,
    [FromBody] object request) =>
{
    var html = await templateService.GetHtml(id, request);

    logger.LogInformation(html);

    var (pdfBytes, workDir) = await latexService.HtmlToLatexToPdf(html);

    var zipBytes = FileService.ZipDirectory(workDir);

    var success = FileService.DeleteDirectory(workDir);

    logger.LogInformation("workDir deleted: {Success}", success);

    return Results.File(zipBytes, contentType: "application/zip", fileDownloadName: $"{FileService.GetRandomFilenameWithoutExtension()}.zip");
})
.Accepts<object>("application/json")
.Produces<byte[]>((int)HttpStatusCode.OK, "application/zip");


app.Run();