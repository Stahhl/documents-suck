
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string corsPolicy = "YOLO";
builder.Services.AddCors(options => options.AddPolicy(corsPolicy, policy => { policy.WithOrigins("*"); }));

var app = builder.Build();

app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/foo", async (ILogger<Program> logger, HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync() ?? ""; 

    logger.LogInformation(body);

    return Results.Ok("accepted by document-service");
}).Accepts<HttpRequest>("text/plain");

app.Run();