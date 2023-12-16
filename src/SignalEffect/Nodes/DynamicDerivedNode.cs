namespace SignalEffect;

internal class DynamicDerivedNode<T> : DerivedNode<T>
where T : notnull
{
    private readonly Func<T> m_Callback;

    private DynamicDerivedNode(ICallTrack track, Func<T> calculation) : base(track)
    {
        m_Callback = calculation;
    }

    protected override void Do(SequenceNumber check)
    {
        var deps = new List<IValueNode>();
        var val = m_Value;
        var prev = Track.State;
        try
        {
            Track.State = new CallState(deps, true, true);
            Visited = true;
            m_Value = m_Callback();
        }
        finally
        {
            Visited = false;
            Track.State = prev;
        }
        Update(deps, true, !Equals(val, m_Value));
    }

    public static Derived<T> Derived(ICallTrack track, Func<T> calculation)
    {
        var d = new DynamicDerivedNode<T>(track, calculation).AsDerived();
        track.Add(d);
        return d;
    }

}
