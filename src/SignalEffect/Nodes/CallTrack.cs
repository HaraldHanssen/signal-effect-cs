
namespace SignalEffect;

internal class CallTrack
{
    private readonly Scope m_Scope;
    private readonly IExecution m_Handler;
    private readonly List<(Node, SequenceNumber)> m_Handles = [];
    private uint m_Depth;
    private bool m_Processing;

    public CallTrack(Scope scope, IExecution handler)
    {
        m_Scope = scope;
        m_Handler = handler;
    }

    public CallState State { get; set; } = new CallState(null, false, false);


    public void Add(IEffect e)
    {
        m_Handler.Changed(null, null, [e]);
    }

    public void Add<T>(IDerived<T> d) where T : notnull
    {
        m_Handler.Changed(null, [d], null);
    }

    public void Changed(IRead read, List<IDerived> deriveds, List<IEffect> effects)
    {
        m_Handler.Changed(read, deriveds, effects);
    }

    public void Enter()
    {
        m_Depth++;
    }

    public void Handle(DependentNode dependentNode, SequenceNumber current)
    {
        m_Handles.Add((dependentNode, current));
    }

    public void Exit()
    {
        m_Depth--;
        if (m_Depth == 0 && !m_Processing)
        {
            try
            {
                m_Processing = true;
                for (var i = 0; i < m_Handles.Count; i++)
                {
                    var (n, s) = m_Handles[i];
                    n.Notify(s);
                }

                // TODO
                // if (diagnostic?.enabled && diagnostic.counters.maxHandles < handles.length) {
                //     diagnostic.counters.maxHandles = handles.length;
                // }

                m_Handles.Clear();
            }
            finally
            {
                m_Processing = false;
            }
        }
    }
}
