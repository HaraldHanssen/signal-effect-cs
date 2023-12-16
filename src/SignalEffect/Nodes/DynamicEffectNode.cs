namespace SignalEffect;

internal class DynamicEffectNode : EffectNode
{
    private readonly Action m_Callback;

    private DynamicEffectNode(ICallTrack track, Action action) : base(track)
    {
        m_Callback = action;
    }

    protected override void Do(SequenceNumber check)
    {
        var deps = new List<IValueNode>();
        //TODO const prev = track;
        try
        {
            //TODO track = { deps, nocall: true, nowrite: true };
            Visited = true;
            m_Callback();
        }
        finally
        {
            Visited = false;
            //TODO track = prev;
        }
        Update(deps, true, false);
    }

    public static Effect Effect(ICallTrack track, Action action) {
        var e = new DynamicEffectNode(track, action).AsEffect();
        //TODO execution.handler.changed(undefined, undefined, [e]);
        return e;
    }
}
