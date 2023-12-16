namespace SignalEffect;

internal abstract class Node
{
    private static NodeId m_NextId = 0;
    private static SequenceNumber m_SeqN = 0;

    public NodeId Id { get; private set; }
    public SequenceNumber Current { get; protected set; }

    protected ICallTrack Track { get; }

    protected InMap? In { get; set; }
    protected OutMap? Out { get; set; }

    protected Node(ICallTrack track, SequenceNumber current)
    {
        Id = NextId();
        Track = track;
        Current = current;
    }

    protected void Notify(SequenceNumber current) {
        //TODO if (diagnostic?.enabled) diagnostic.counters.notify++;
        // Notify execution handler
        List<IDerived> deriveds = [];
        List<IEffect> effects = [];

        // Logic:
        // Traverse out and alert dependants. Stop traversal on nodes that already are alerted.
        // Send this._out to execution handler.

        List<Node> traverse = [this];
        for (var i = 0; i < traverse.Count; i++) {
            var source = traverse[i];
            if (source.Out == null) continue;
            foreach (var (k, v) in source.Out) {
                // clean
                if (!v.TryGetTarget(out var d) || d.Dropped) {
                    source.Out!.Remove(k);
                    continue;
                }

                // traverse dependencies for alert
                if (d.Alert(current)) {
                    if (d is DerivedNode) {
                        traverse.Add(d);
                    }
                };

                // gather this._out
                if (i == 0) {
                    if (d is DerivedNode dd) {
                        deriveds.Add(dd.AsDerived());
                    }
                    if (d is EffectNode ee) {
                        effects.Add(ee.AsEffect());
                    }
                }
            }
        }

        //TODO if (diagnostic?.enabled) diagnostic.counters.notifyDeps += deriveds.length + effects.length;

        var prev = Track.State;
        try {
            Track.State = new CallState(null, false, false);
            if (this is SignalNode sn) {
                Track.Changed(sn.AsReadable(), deriveds, effects);
            }
            else if (this is DerivedNode dn) {
                Track.Changed(dn.AsDerived(), deriveds, effects);
            }
        }
        finally {
            Track.State = prev;
        }
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
