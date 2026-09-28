using EvalTarget.Services;
using Xunit;

namespace EvalTarget.Tests;

public sealed class OutputFormatterTests
{
    private readonly OutputFormatter _sut = new();

    [Fact]
    public void Format_Upper_TrimsAndUpperCases()
    {
        Assert.Equal("HELLO", _sut.Format("  hello  ", "upper"));
    }

    [Fact]
    public void Format_Null_ReturnsPlaceholder()
    {
        Assert.Equal("(empty)", _sut.Format(null, "text"));
    }

    [Fact]
    public void Format_Empty_ReturnsPlaceholder()
    {
        Assert.Equal("(empty)", _sut.Format(string.Empty, "text"));
    }

    [Fact]
    public void Format_Lower_LowercasesAndTrims()
    {
        Assert.Equal("hello", _sut.Format("  HeLLo  ", "lower"));
    }

    [Fact]
    public void Format_UnknownFormat_ReturnsTrimmed()
    {
        Assert.Equal("hello", _sut.Format("  hello  ", "text"));
    }
}
