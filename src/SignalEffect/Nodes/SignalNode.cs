namespace SignalEffect;

internal abstract class SignalNode : Node
{
    protected SignalNode(CallTrack track) : base(track, NextN())
    {
    }

    public abstract IRead AsReadable();
}

internal class SignalNode<T> : SignalNode, IValueNode
where T : notnull
{
    private T m_Value;
    public SignalNode(CallTrack track, T value) : base(track)
    {
        m_Value = value;
        Out = [];
    }

    public T Value => m_Value;

    public IWrite<T> AsWritable()
    {
        return new Write<T>(Id, this, RFun, WFun);
    }

    public override IRead<T> AsReadable()
    {
        return new Read<T>(Id, this, RFun);
    }

    public object GetValue(SequenceNumber check)
    {
        return Value;
    }

    private T RFun()
    {
        Track.State.Dependencies?.Add(this);
        return m_Value;
    }

    private void WFun(T value)
    {
        if (Track.State.NoWrite) throw new ReentryException(ReentryException.ERR_WRITE);
        if (Equals(m_Value, value)) return;
        Track.Enter();
        m_Value = value;
        Current = NextN();
        //TODO handle(this, this.current);
        Track.Exit();
    }

    private void MFun(Action<T> manipulate)
    {
        if (Track.State.NoWrite) throw new ReentryException(ReentryException.ERR_WRITE);
        Track.Enter();
        manipulate(m_Value);
        Current = NextN();
        //TODO handle(this, this.current);
        Track.Exit();
    }

    public static IWrite<T> Signal(CallTrack track, T initial)
    {
        return new SignalNode<T>(track, initial).AsWritable();
    }

    public static IRead<T> Readonly(IWrite<T> signal)
    {
        return SignalNode<T>.ToNode(signal).AsReadable();
    }

    public static void Modify(IWrite<T> signal, Action<T> manipulate)
    {
        SignalNode<T>.ToNode(signal).MFun(manipulate);
    }

    private static SignalNode<T> ToNode(IWrite<T> signal)
    {
        var s = (signal as Primitive)?.Self as SignalNode<T>;
        if (s == null)
        {
            throw new SignalException("Expected a writable signal.");
        }
        return s;
    }

}
