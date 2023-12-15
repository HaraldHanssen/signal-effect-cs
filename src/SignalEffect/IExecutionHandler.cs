namespace SignalEffect;

public interface IExecutionHandler
{
    public void Changed(IReadOnlySignal? source, IList<IDerivedSignal>? deriveds, IList<IEffect>? effects);
}