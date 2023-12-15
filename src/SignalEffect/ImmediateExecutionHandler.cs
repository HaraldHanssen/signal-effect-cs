namespace SignalEffect;

public class ImmediateExecutionHandler : IExecutionHandler
{
    public void Changed(IReadOnlySignal? _, IList<IDerivedSignal>? deriveds, IList<IEffect>? effects)
    {
        if (deriveds != null)
        {
            foreach (var item in deriveds)
            {
                item.GetValue();
            }
        }
        if (effects != null)
        {
            foreach (var item in effects)
            {
                item.Call();
            }
        }
    }
}