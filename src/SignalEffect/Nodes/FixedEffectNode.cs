namespace SignalEffect;

internal class FixedEffectNode : EffectNode
{
    private readonly List<IValueNode> m_Deps;
    private readonly Action<object[]> m_Callback;

    private FixedEffectNode(CallTrack track, List<IValueNode> dependencies, Action<object[]> action) : base(track)
    {
        m_Deps = dependencies;
        m_Callback = action;
        Init(dependencies);
    }

    protected override void Do(SequenceNumber check)
    {
        var values = m_Deps.Select(x => x.GetValue(check)).ToArray();
        var prev = Track.State;
        try
        {
            Track.State = new CallState(null, true, true);
            Visited = true;
            m_Callback(values);
        }
        finally
        {
            Visited = false;
            Track.State = prev;
        }
        Update(m_Deps, false, false);
    }

    public static IEffect Effect(CallTrack track, List<IValueNode> dependencies, Action<object[]> action) {
        var e = new FixedEffectNode(track, dependencies, action).AsEffect();
        track.Add(e);
        return e;
    }
}