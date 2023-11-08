namespace Gee.CodeAnalysis;

public sealed class Diagnostic
{
    public TextSpan Span { get; }
    public string Message { get; }
    public override string ToString() => Message;

    public Diagnostic(TextSpan span, string message)
    {
        Span = span;
        Message = message;
    }
}