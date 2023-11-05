namespace compiler.CodeAnalysis.Syntax;


public enum SyntaxKind
{
    //Tokens
    BadToken,
    EndOfFileToken,
    WhitespaceToken,
    NumberToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    IdentifierToken,
    
    //Keywords
    TrueKeyword,
    FalseKeyword,
    
    //Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression
}





