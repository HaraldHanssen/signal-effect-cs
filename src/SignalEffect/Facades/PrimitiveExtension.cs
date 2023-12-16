namespace SignalEffect;

internal static class PrimitiveExtension
{
    public static IValueNode ValueNode(this IRead r)
    {
        return (IValueNode)((Primitive)r).Self;
    }
}