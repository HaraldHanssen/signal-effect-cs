namespace SignalEffect;

internal abstract class DerivedNode : DependentNode
{
    protected DerivedNode(CallTrack track) : base(track)
    {
    }

    public abstract IDerived AsDerived();
}

internal abstract class DerivedNode<T> : DerivedNode, IValueNode
where T : notnull
{
    protected T? m_Value;

    protected DerivedNode(CallTrack track) : base(track)
    {
        In = [];
        Out = [];
    }

    public override IDerived<T> AsDerived()
    {
        return new Derived<T>(Id, this, Fun);
    }

    public T Value(SequenceNumber check)
    {
        if (Visited) throw new ReentryException(ReentryException.ERR_LOOP);
        Track.State.Dependencies?.Add(this);
        if (!Dropped && check > Checked)
        {
            if (Dirty)
            {
                try
                {
                    Track.Enter();
                    Do(check);
                    Dirty = false;
                }
                finally
                {
                    Track.Exit();
                }
            }

            Checked = check;
        }
        return (m_Value ?? default)!;
    }

    public object GetValue(SequenceNumber check)
    {
        return Value(check);
    }

    protected abstract void Do(SequenceNumber check);

    private T Fun()
    {
        return Value(CurrN());
    }

}
