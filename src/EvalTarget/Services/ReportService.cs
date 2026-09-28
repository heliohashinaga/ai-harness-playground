using Microsoft.Extensions.Options;
using EvalTarget.Models;

namespace EvalTarget.Services;

public sealed class ReportService
{
    private readonly OutputFormatter _formatter;
    private readonly EvalOptions _options;

    public ReportService(OutputFormatter formatter, IOptions<EvalOptions> options)
    {
        _formatter = formatter;
        _options = options.Value;
    }

    public ReportResult CreateReport(ReportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title must not be empty.", nameof(request));
        }

        var heading = request.Title;
        var format = request.Format ?? _options.DefaultFormat;
        var body = _formatter.Format(request.Body, format);
        return new ReportResult(heading, body, format);
    }

    public static string SlugFor(string title)
    {
        var trimmed = title.Trim().ToLowerInvariant();
        var chars = trimmed.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var slug = new string(chars);
        while (slug.Contains("--", StringComparison.Ordinal))
        {
            slug = slug.Replace("--", "-", StringComparison.Ordinal);
        }

        return slug.Trim('-');
    }
}
