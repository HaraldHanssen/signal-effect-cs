namespace SignalEffect;

internal class Read<T> : Primitive, IRead<T>
where T: notnull
{
    internal Read(NodeId id, DerivedNode<T> self, Func<T> get) : base(id, self)
    {
        Get = get;
    }

    internal Read(NodeId id, SignalNode<T> self, Func<T> get) : base(id, self)
    {
        Get = get;
    }

    public Func<T> Get { get; }

    public Func<object> GetValue => () => Get();
}
