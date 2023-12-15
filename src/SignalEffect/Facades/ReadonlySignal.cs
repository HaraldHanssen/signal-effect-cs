namespace SignalEffect;

internal class ReadonlySignal<T> : Primitive, IReadOnlySignal<T>
where T: notnull
{
    internal ReadonlySignal(NodeId id, DerivedNode<T> self, Func<T> get) : base(id, self)
    {
        Get = get;
    }

    internal ReadonlySignal(NodeId id, SignalNode<T> self, Func<T> get) : base(id, self)
    {
        Get = get;
    }

    public Func<T> Get { get; }

    public Func<object> GetValue => () => Get();
}
