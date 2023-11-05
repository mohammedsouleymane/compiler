namespace compiler.CodeAnalysis.Syntax;

public sealed class SyntaxToken : SyntaxNode
{
    public override SyntaxKind Kind { get;}
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>();
    }

    public int Position { get; set; }
    public string? Text { get; set; }
    public object? Value { get; set; }
    public SyntaxToken(SyntaxKind kind, int position, string? text, object? value)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }
}
