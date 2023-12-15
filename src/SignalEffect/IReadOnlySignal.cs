namespace SignalEffect;

public interface IReadOnlySignal : IPrimitive {
    public Func<object> GetValue { get; }
}

public interface IReadOnlySignal<T> : IReadOnlySignal {
    public Func<T> Get { get; }
}
