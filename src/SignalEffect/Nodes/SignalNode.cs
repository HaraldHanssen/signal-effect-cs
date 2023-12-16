namespace SignalEffect;

internal class SignalNode<T> : Node, IValueNode
where T : notnull
{
    private readonly ICallTrack m_Track;
    private T m_Value;
    public SignalNode(ICallTrack track, T value) : base(NextN())
    {
        m_Track = track;
        m_Value = value;
        Out = [];
    }

    public T Value => m_Value;

    public IWrite<T> AsWritable()
    {
        return new Write<T>(Id, this, RFun, WFun);
    }

    public IRead<T> AsReadable()
    {
        return new Read<T>(Id, this, RFun);
    }

    public object GetValue(SequenceNumber check)
    {
        return Value;
    }

    private T RFun()
    {
        //TODO track.deps?.push(this);
        return m_Value;
    }

    private void WFun(T value)
    {
        //TODO if (track.nowrite) throw new ReentryError(ERR_WRITE);
        if (Equals(m_Value, value)) return;
        //TODO enter();
        m_Value = value;
        Current = NextN();
        //TODO handle(this, this.current);
        //TODO exit();
    }

    private void MFun(Action<T> manipulate)
    {
        //TODO if (track.nowrite) throw new ReentryError(ERR_WRITE);
        //TODO enter();
        manipulate(m_Value);
        Current = NextN();
        //TODO handle(this, this.current);
        //TODO exit();
    }

    public static IWrite<T> Signal(ICallTrack track, T initial)
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
