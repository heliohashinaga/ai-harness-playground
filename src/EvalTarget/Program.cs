using EvalTarget.Models;
using EvalTarget.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EvalOptions>(builder.Configuration.GetSection("EvalTarget"));
builder.Services.AddSingleton<OutputFormatter>();
builder.Services.AddSingleton<ReportService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/reports", (ReportRequest request, ReportService service) =>
{
    try
    {
        var result = service.CreateReport(request);
        return Results.Ok(result);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

public partial class Program;
