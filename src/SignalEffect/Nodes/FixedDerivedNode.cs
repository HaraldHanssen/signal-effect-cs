namespace SignalEffect;

internal class FixedDerivedNode<T> : DerivedNode<T>
where T : notnull
{
    private readonly List<IValueNode> m_Deps;
    private readonly Func<object[], T> m_Callback;

    private FixedDerivedNode(List<IValueNode> dependencies, Func<object[], T> calculation)
    {
        m_Deps = dependencies;
        m_Callback = calculation;
        Init(dependencies);
    }

    protected override void Do(SequenceNumber check)
    {
        var values = m_Deps.Select(x => x.GetValue(check)).ToArray();
        var val = m_Value;
        //TODO const prev = track;
        try
        {
            //TODO track = { deps: undefined, nocall: true, nowrite: true };
            Visited = true;
            m_Value = m_Callback(values);
        }
        finally
        {
            Visited = false;
            //TODO track = prev;
        }
        Update(m_Deps, false, !Equals(val, m_Value));
    }

    public static DerivedSignal<T> Derived(List<IValueNode> dependencies, Func<object[], T> calculation)
    {
        var d = new FixedDerivedNode<T>(dependencies, calculation).AsDerived();
        //TODO execution.handler.changed(undefined, [d], undefined);
        return d;
    }
}