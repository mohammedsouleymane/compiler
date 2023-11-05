using compiler.CodeAnalysis.Binding;
using compiler.CodeAnalysis.Syntax;

namespace compiler.CodeAnalysis;

internal sealed class Evaluator
{
    private readonly BoundExpression _root;

    public Evaluator(BoundExpression root)
    {
        _root = root;
    }

    public object Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private object EvaluateExpression(BoundExpression node)
    {
        switch (node)
        {
            case BoundLiteralExpression n:
                return  n.Value;
            case BoundUnaryExpression u:
            {
                var operand = (int)EvaluateExpression(u.Operand);
                return u.OperatorKind switch
                {
                    BoundUnaryOperatorKind.Negation => -operand,
                    BoundUnaryOperatorKind.Identity => operand,
                    _ => throw new Exception($"Unexpected node {u.OperatorKind}")
                };
            }
            case BoundBinaryExpression b:
            {
                var left = (int) EvaluateExpression(b.Left);
                var right =(int) EvaluateExpression(b.Right);
                return b.OperatorKind switch
                {
                    BoundBinaryOperatorKind.Addition => left + right,
                    BoundBinaryOperatorKind.Subtraction => left - right,
                    BoundBinaryOperatorKind.Multiplication => left * right,
                    BoundBinaryOperatorKind.Division => left / right,
                    _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
                };
            }
            default:
                throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}