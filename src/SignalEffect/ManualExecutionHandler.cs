namespace SignalEffect;

public class ManualExecutionHandler : IExecutionHandler
{
    public void Changed(IReadOnlySignal? source, IList<IDerivedSignal>? deriveds, IList<IEffect>? effects)
    {
    }

    public void Update(IEnumerable<IDerivedSignal> deriveds) {
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