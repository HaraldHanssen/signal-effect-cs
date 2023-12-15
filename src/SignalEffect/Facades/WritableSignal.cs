namespace SignalEffect;

internal class WritableSignal<T> : ReadonlySignal<T>, IWritableSignal<T>
where T : notnull
{
    internal WritableSignal(NodeId id, SignalNode<T> self, Func<T> get, Action<T> set) : base(id, self, get)
    {
        Set = set;
    }
    public Action<T> Set { get; }

    public Action<object> SetValue => (x) => Set((T)x);
}
