namespace SignalEffect;

internal class SignalNode<T> : Node, IValueNode
where T: notnull
{
    private T m_Value;
    public SignalNode(T value) : base(NextN())
    {
        m_Value = value;
        Out = [];
    }

    public T Value => m_Value;

    public object GetValue(SequenceNumber check) {
        return Value;
    }
}
