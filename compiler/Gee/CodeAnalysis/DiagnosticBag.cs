using System.Collections;
using Gee.CodeAnalysis.Syntax;

namespace Gee.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();
    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(DiagnosticBag diagnostics)
    {
        _diagnostics.AddRange(diagnostics);
    }
    private void Report(TextSpan span, string message)
    {
        var diagnostic = new Diagnostic(span, message);
        _diagnostics.Add(diagnostic);
    }


    public void ReportInvalidNumber(TextSpan span, string text, Type type)
    {
        var message = $"The number {text} isn't valid {type}.";
        Report(span, message);
    }

    public void ReportBadCharacter(int position, char character)
    {
        var message = $"Bad character in input: {character}.";
        Report(new TextSpan(position, 1), message);
    }

    public void ReportUnexpectedToken(TextSpan currentSpan, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"Unexpected token <{actualKind}> expected <{expectedKind}>.";
        Report(currentSpan, message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string? operatorText, Type type)
    {
        var message = $"Unary operator '{operatorText}' is not defined for type {type}.";
        Report(span, message);
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, string? operatorText, Type leftType, Type rightType)
    {
        var message = $"Binary operator '{operatorText}' is not defined for types {leftType} and {rightType}.";
        Report(span, message);
    }
}