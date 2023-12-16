namespace SignalEffect;

public interface IWrite {
    public Action<object> SetValue { get; }
}


public interface IWrite<T> : IWrite {
    public Action<T> Set { get; }
}
