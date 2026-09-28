using EvalTarget.Models;
using EvalTarget.Services;

namespace EvalTarget.Worker;

public sealed class ReportWorker : BackgroundService
{
    private readonly ReportService _service;
    private readonly ILogger<ReportWorker> _logger;

    public ReportWorker(ReportService service, ILogger<ReportWorker> logger)
    {
        _service = service;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var report = _service.CreateReport(new ReportRequest("Heartbeat", "worker alive", "text"));
            _logger.LogInformation("Worker report: {Heading}", report.Heading);
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
