using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EvalTarget.Models;
using EvalTarget.Worker;
using Xunit;

namespace EvalTarget.Tests;

public sealed class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_Reports_ReturnsReport()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/reports", new ReportRequest("Hello", "world", "text"));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ReportResult>();
        Assert.NotNull(result);
        Assert.Equal("Hello", result!.Heading);
    }

    [Fact]
    public void Worker_IsRegistered()
    {
        using var scope = _factory.Services.CreateScope();
        var hosted = scope.ServiceProvider.GetServices<IHostedService>();
        Assert.Contains(hosted, h => h is ReportWorker);
    }
}
