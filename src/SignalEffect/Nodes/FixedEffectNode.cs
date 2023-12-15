namespace SignalEffect;

internal class FixedEffectNode : EffectNode
{
    private readonly List<IValueNode> m_Deps;
    private readonly Action<object[]> m_Callback;

    private FixedEffectNode(List<IValueNode> dependencies, Action<object[]> action)
    {
        m_Deps = dependencies;
        m_Callback = action;
        Init(dependencies);
    }

    protected override void Do(SequenceNumber check)
    {
        var values = m_Deps.Select(x => x.GetValue(check)).ToArray();
        //TODO const prev = track;
        try
        {
            //TODO track = { deps: undefined, nocall: true, nowrite: true };
            Visited = true;
            m_Callback(values);
        }
        finally
        {
            Visited = false;
            //TODO track = prev;
        }
        Update(m_Deps, false, false);
    }

    public static Effect Effect(List<IValueNode> dependencies, Action<object[]> action) {
        var e = new FixedEffectNode(dependencies, action).AsEffect();
        //TODO execution.handler.changed(undefined, undefined, [e]);
        return e;
    }
}