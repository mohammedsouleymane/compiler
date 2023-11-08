namespace Gee.CodeAnalysis.Binding;

internal sealed class BoundLiteralExpression : BoundExpression
{
    public BoundLiteralExpression(object value)
    {
        Value = value;
    }

    public object Value { get; set; }
    public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public override Type Type => Value.GetType();
}