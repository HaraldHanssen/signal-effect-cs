namespace SignalEffect;

public interface IWritableSignal {
    public Action<object> SetValue { get; }
}


public interface IWritableSignal<T> : IWritableSignal {
    public Action<T> Set { get; }
}
