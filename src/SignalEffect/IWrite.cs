namespace SignalEffect;

public interface IWrite
{
    public Action<object> SetValue { get; }
}


public interface IWrite<T> : IWrite, IRead<T>
{
    public Action<T> Set { get; }
}
