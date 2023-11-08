namespace Gee.CodeAnalysis.Syntax;

public sealed class UnaryExpressionSyntax : ExpressionSyntax
{
    public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
    {
        OperatorToken = operatorToken;
        Operand = operand;
    }

    public ExpressionSyntax Operand { get; set; }

    public SyntaxToken OperatorToken { get; set; }
    

    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OperatorToken;
        yield return Operand;
    }
}