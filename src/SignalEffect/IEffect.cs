namespace SignalEffect;

public interface IEffect : IPrimitive
{
    public Action Call { get; }
}