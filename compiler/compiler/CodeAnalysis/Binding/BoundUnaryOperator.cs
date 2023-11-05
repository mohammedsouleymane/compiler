using compiler.CodeAnalysis.Syntax;

namespace compiler.CodeAnalysis.Binding;

internal sealed class BoundUnaryOperator
{
    public SyntaxKind SyntaxKind { get; }
    public BoundUnaryOperatorKind Kind { get; }
    public Type OperandType { get; }
    public Type ResultType { get; }

    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType)
        :this (syntaxKind, kind, operandType, operandType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
    }
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        ResultType = resultType;
    }

    private static BoundUnaryOperator[] _operators =
    {
        new (SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
        new (SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(bool)),
        new (SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(bool)),
    };

    public static BoundUnaryOperator? Bind(SyntaxKind syntaxKind, Type operandType)
    {
        return _operators.FirstOrDefault(op => op.SyntaxKind == syntaxKind && op.OperandType == operandType);
    } 
}