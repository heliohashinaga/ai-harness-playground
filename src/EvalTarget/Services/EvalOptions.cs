namespace EvalTarget.Services;

public sealed class EvalOptions
{
    public string DefaultFormat { get; set; } = "text";
    public int MaxTitleLength { get; set; } = 80;
}
