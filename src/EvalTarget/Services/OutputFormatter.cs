namespace EvalTarget.Services;

public sealed class OutputFormatter
{
    public string Format(string? input, string? format)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "(empty)";
        }

        if (string.Equals(format, "upper", StringComparison.OrdinalIgnoreCase))
        {
            return input.ToUpperInvariant();
        }

        if (string.Equals(format, "lower", StringComparison.OrdinalIgnoreCase))
        {
            return input.Trim().ToLowerInvariant();
        }

        return input.Trim();
    }
}
