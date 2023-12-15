namespace SignalEffect;

internal interface IValueNode
{
    public SequenceNumber Current { get; }

    public object GetValue(SequenceNumber check);

}