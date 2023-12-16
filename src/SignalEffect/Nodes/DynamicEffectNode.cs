namespace SignalEffect;

internal class DynamicEffectNode : EffectNode
{
    private readonly Action m_Callback;

    private DynamicEffectNode(CallTrack track, Action action) : base(track)
    {
        m_Callback = action;
    }

    protected override void Do(SequenceNumber check)
    {
        var deps = new List<IValueNode>();
        var prev = Track.State;
        try
        {
            Track.State = new CallState(deps, true, true);
            Visited = true;
            m_Callback();
        }
        finally
        {
            Visited = false;
            Track.State = prev;
        }
        Update(deps, true, false);
    }

    public static IEffect Effect(CallTrack track, Action action) {
        var e = new DynamicEffectNode(track, action).AsEffect();
        track.Add(e);
        return e;
    }
}
