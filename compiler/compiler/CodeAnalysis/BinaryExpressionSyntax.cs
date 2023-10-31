namespace compiler.CodeAnalysis;

sealed class BinaryExpressionSyntax : ExpressionSyntax
{
    public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
    }

    public ExpressionSyntax Right { get; set; }

    public SyntaxToken OperatorToken { get; set; }

    public ExpressionSyntax Left { get; set; }

    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return OperatorToken;
        yield return Right;
    }
}