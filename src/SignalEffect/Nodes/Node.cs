namespace SignalEffect;

internal abstract class Node
{
    private static NodeId m_NextId = 0;
    private static SequenceNumber m_SeqN = 0;

    public NodeId Id { get; private set; }
    public SequenceNumber Current { get; protected set; }

    protected InMap? In { get; set; }
    protected OutMap? Out { get; set; }

    protected Node(SequenceNumber current)
    {
        Id = NextId();
        Current = current;
    }
    
    protected void Handle(DependentNode dependentNode, SequenceNumber current)
    {
        throw new NotImplementedException();
    }

    internal static void Link(IValueNode source, DependentNode target)
    {
        var s = (Node)source;
        target.In![s.Id] = source;
        s.Out![target.Id] = new WeakReference<DependentNode>(target);
    }

    internal static void Unlink(IValueNode source, DependentNode target)
    {
        var s = (Node)source;
        target.In!.Remove(s.Id);
        s.Out!.Remove(target.Id);
    }

    private static NodeId NextId()
    {
        return Interlocked.Increment(ref m_NextId);
    }

    protected static SequenceNumber NextN()
    {
        return Interlocked.Increment(ref m_SeqN);
    }

    protected static SequenceNumber CurrN() {
        return Interlocked.Read(ref m_SeqN);
    }
}
