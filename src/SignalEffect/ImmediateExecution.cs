namespace SignalEffect;

public class ImmediateExecution : IExecution
{
    public void Changed(IRead? _, IList<IDerived>? deriveds, IList<IEffect>? effects)
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