namespace SignalEffect;

internal abstract class DerivedNode<T> : DependentNode, IValueNode
where T : notnull
{
    protected T? m_Value;

    protected DerivedNode(ICallTrack track) : base()
    {
        In = [];
        Out = [];
        Track = track;
    }

    protected ICallTrack Track { get; }

    public Derived<T> AsDerived() {
        return new Derived<T>(Id, this, Fun);
    }


    public T Value(SequenceNumber check)
    {
        if (Visited) throw new ReentryException(ReentryException.ERR_LOOP);
        //TODO track.deps?.push(this);
        if (!Dropped && check > Checked)
        {
            if (Dirty)
            {
                try
                {
                    //TODO enter();
                    Do(check);
                    Dirty = false;
                }
                finally
                {
                    //TODO exit();
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
