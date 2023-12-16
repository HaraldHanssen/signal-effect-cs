namespace SignalEffect;

internal class FixedDerivedNode<T> : DerivedNode<T>
where T : notnull
{
    private readonly List<IValueNode> m_Deps;
    private readonly Func<object[], T> m_Callback;

    private FixedDerivedNode(ICallTrack track, List<IValueNode> dependencies, Func<object[], T> calculation) : base(track)
    {
        m_Deps = dependencies;
        m_Callback = calculation;
        Init(dependencies);
    }

    protected override void Do(SequenceNumber check)
    {
        var values = m_Deps.Select(x => x.GetValue(check)).ToArray();
        var val = m_Value;
        var prev = Track.State;
        try
        {
            Track.State = new CallState(null, true, true);
            Visited = true;
            m_Value = m_Callback(values);
        }
        finally
        {
            Visited = false;
            Track.State = prev;
        }
        Update(m_Deps, false, !Equals(val, m_Value));
    }

    public static IDerived<T> Derived(ICallTrack track, List<IValueNode> dependencies, Func<object[], T> calculation)
    {
        var d = new FixedDerivedNode<T>(track, dependencies, calculation).AsDerived();
        track.Add(d);
        return d;
    }
}