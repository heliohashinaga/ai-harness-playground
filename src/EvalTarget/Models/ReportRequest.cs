namespace EvalTarget.Models;

public sealed record ReportRequest(string Title, string Body, string? Format = null);
