namespace SignalEffect;

internal abstract class DependentNode : Node
{
    protected DependentNode(CallTrack track) : base(track, 0)
    {
    }

    public bool Dirty { get; protected set; } = true;
    protected SequenceNumber Checked { get; set; }
    protected bool Visited { get; set; }
    public bool Dropped { get; private set; }

    public bool Alert(SequenceNumber current)
    {
        var wasDirty = Dirty;
        Dirty = current > Current;
        return wasDirty != Dirty;
    }

    public void Drop() => Dropped = true;

    protected void Init(IList<IValueNode> deps)
    {
        foreach (var dep in deps)
        {
            Link(dep, this);
        }
    }


    protected void Update(IEnumerable<IValueNode> deps, bool relink, bool notify)
    {
        if (relink)
        {
            foreach (var dep in In!.Values)
            {
                Unlink(dep, this);
            }
            foreach (var dep in deps)
            {
                Link(dep, this);
                if (dep.Current > Current) Current = dep.Current;
            }
        }
        else
        {
            foreach (var dep in deps)
            {
                Link(dep, this);
                if (dep.Current > Current) Current = dep.Current;
            }
        }

        if (notify)
        {
            Track.Handle(this, Current);
        }
    }
}
