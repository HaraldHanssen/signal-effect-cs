namespace SignalEffect;

public interface IExecution
{
    public void Changed(IRead? source, IList<IDerived>? deriveds, IList<IEffect>? effects);
}
