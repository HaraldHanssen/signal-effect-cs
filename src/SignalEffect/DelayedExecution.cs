namespace SignalEffect;

public class DelayedExecution : IExecution
{
    private Dictionary<NodeId, IDerived> m_Deriveds = [];
    private Dictionary<NodeId, IEffect> m_Effects = [];

    public void Changed(IRead? _, IList<IDerived>? deriveds, IList<IEffect>? effects)
    {
        if (deriveds != null)
        {
            foreach (var item in deriveds)
            {
                m_Deriveds[item.Id] = item;
            }
        }
        if (effects != null)
        {
            foreach (var item in effects)
            {
                m_Effects[item.Id] = item;
            }
        }        
    }

    public (IEnumerable<IDerived>, IEnumerable<IEffect>) Update() {
        var d = m_Deriveds;
        var e = m_Effects;
        m_Deriveds = [];
        m_Effects = [];
        foreach (var item in d.Values)
        {
            item.GetValue();
        }
        foreach (var item in e.Values)
        {
            item.Call();
        }
        return  (d.Values, e.Values);
    }

}