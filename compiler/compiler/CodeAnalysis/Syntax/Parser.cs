namespace compiler.CodeAnalysis.Syntax;

internal sealed class Parser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;
    private readonly List<string> _diagnostics = new();
    public IEnumerable<string> Diagnostics => _diagnostics;
    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(text);
        SyntaxToken token;
        do
        {
            token = lexer.Lex();
            if (token.Kind is not SyntaxKind.WhitespaceToken and not SyntaxKind.BadToken)
                tokens.Add(token);
        } while (token.Kind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();
        _diagnostics.AddRange(lexer.Diagnostics);
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        return index >= _tokens.Length ? _tokens[^1] : _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }
    private SyntaxToken MatchToken(SyntaxKind kind)
    {
        if (Current.Kind == kind)
            return NextToken();
        
        _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}> expected <{kind}>");
        return new SyntaxToken(kind, Current.Position, null ,null);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_diagnostics, expression, endOfFileToken);
    }

    private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left; 
        var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else
            left = ParsePrimaryExpression();
        
            
        while (true)
        {
            var precedence = Current.Kind.GetBinaryOperatorPrecedence();
            if (precedence == 0  || precedence <= parentPrecedence)
                break;
            var operatorToken = NextToken();
            var right = ParseExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }
    
    
    private ExpressionSyntax ParsePrimaryExpression()
    {
        if (Current.Kind is SyntaxKind.OpenParenthesisToken)
        {
            var left = NextToken();
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }
        var numberToken = MatchToken(SyntaxKind.NumberToken);
        return new LiteralExpressionSyntax(numberToken);
    }
}