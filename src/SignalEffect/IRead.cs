namespace SignalEffect;

public interface IRead : IPrimitive
{
    public Func<object> GetValue { get; }
}

public interface IRead<T> : IRead
{
    public Func<T> Get { get; }
}
