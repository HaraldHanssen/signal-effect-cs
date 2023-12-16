namespace SignalEffect;

public class ManualExecution : IExecution
{
    public void Changed(IRead? source, IList<IDerived>? deriveds, IList<IEffect>? effects)
    {
    }

    public void Update(IEnumerable<IDerived> deriveds) {
        foreach (var item in deriveds)
        {
            item.GetValue();
        }
    }

    public void Update(IEnumerable<IEffect> effects) {
        foreach (var item in effects)
        {
            item.Call();
        }
    }
}