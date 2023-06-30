
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using DocumentService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<TemplateService>();

builder.Services.AddScoped<FileService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/document/{id}/html", async (
    [FromServices] ILogger<Program> logger,
    [FromServices] TemplateService service,
    [FromRoute] string id,
    [FromBody] object request) =>
{
    var html = await service.GetHtml(id, request);

    logger.LogInformation(html);

    return Results.Extensions.Html(html);
})
.Accepts<object>("application/json")
.Produces<string>((int)HttpStatusCode.OK, "text/html");

app.MapPost("/document/{id}/pdf", async (
    [FromServices] ILogger<Program> logger,
    [FromServices] TemplateService service,
    [FromServices] FileService fileService,
    [FromRoute] string id,
    [FromBody] object request) =>
{
    var html = await service.GetHtml(id, request);

    logger.LogInformation(html);

    var file = await fileService.GetFile(html);

    return Results.File(file, contentType: "text/plain", fileDownloadName: "foo.txt");
})
.Accepts<object>("application/json")
.Produces<byte[]>((int)HttpStatusCode.OK, "text/pdf");

app.Run();