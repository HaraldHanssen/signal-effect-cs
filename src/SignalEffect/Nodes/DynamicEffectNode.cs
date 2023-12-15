namespace SignalEffect;

internal class DynamicEffectNode : EffectNode
{
    private readonly Action m_Callback;

    private DynamicEffectNode(Action action)
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

    public static Effect Effect(Action action) {
        var e = new DynamicEffectNode(action).AsEffect();
        //TODO execution.handler.changed(undefined, undefined, [e]);
        return e;
    }
}
