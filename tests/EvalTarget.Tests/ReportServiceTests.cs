using Microsoft.Extensions.Options;
using EvalTarget.Models;
using EvalTarget.Services;
using Xunit;

namespace EvalTarget.Tests;

public sealed class ReportServiceTests
{
    private static ReportService Build(EvalOptions? options = null)
    {
        var opts = options ?? new EvalOptions { DefaultFormat = "text", MaxTitleLength = 80 };
        return new ReportService(new OutputFormatter(), Options.Create(opts));
    }

    [Fact]
    public void CreateReport_UsesTitleAsHeading()
    {
        var sut = Build();
        var result = sut.CreateReport(new ReportRequest("Hello", "body", "text"));
        Assert.Equal("Hello", result.Heading);
    }

    [Fact]
    public void CreateReport_TruncatesLongTitleToMaxLength()
    {
        var sut = Build(new EvalOptions { DefaultFormat = "text", MaxTitleLength = 80 });
        var longTitle = new string('a', 100);
        var result = sut.CreateReport(new ReportRequest(longTitle, "body", "text"));
        Assert.Equal(80, result.Heading.Length);
    }

    [Fact]
    public void CreateReport_RejectsEmptyTitle()
    {
        var sut = Build();
        Assert.Throws<ArgumentException>(() => sut.CreateReport(new ReportRequest("  ", "body", "text")));
    }

    [Fact]
    public void CreateReport_AppliesDefaultFormatFromOptions()
    {
        var sut = Build(new EvalOptions { DefaultFormat = "lower", MaxTitleLength = 80 });
        var result = sut.CreateReport(new ReportRequest("T", "  ABC  ", null));
        Assert.Equal("lower", result.Format);
        Assert.Equal("abc", result.Body);
    }

    [Fact]
    public void SlugFor_LowercasesAndHyphenates()
    {
        Assert.Equal("hello-world", ReportService.SlugFor("  Hello World!  "));
    }
}
