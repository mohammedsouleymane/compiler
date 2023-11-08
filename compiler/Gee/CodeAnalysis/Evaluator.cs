using Gee.CodeAnalysis.Binding;

namespace Gee.CodeAnalysis;

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
                var operand = EvaluateExpression(u.Operand);
                return u.Op.Kind switch
                {
                    BoundUnaryOperatorKind.Negation => -(int)operand,
                    BoundUnaryOperatorKind.Identity => (int)operand,
                    BoundUnaryOperatorKind.LogicalNegation => !(bool)operand,
                    _ => throw new Exception($"Unexpected node {u.Op.Kind}")
                };
            }
            case BoundBinaryExpression b:
            {
                var left =  EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                return b.Op.Kind switch
                {
                    BoundBinaryOperatorKind.Addition => (int) left + (int)right,
                    BoundBinaryOperatorKind.Subtraction => (int)left -(int) right,
                    BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
                    BoundBinaryOperatorKind.Division => (int)left / (int)right,
                    BoundBinaryOperatorKind.LogicalAnd => (bool) left && (bool) right,
                    BoundBinaryOperatorKind.LogicalOr => (bool) left || (bool) right,
                    BoundBinaryOperatorKind.Equals => Equals(left, right),
                    BoundBinaryOperatorKind.NotEquals => !Equals(left, right),
                    _ => throw new Exception($"Unexpected binary operator {b.Op.Kind}")
                };
            }
            default:
                throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}