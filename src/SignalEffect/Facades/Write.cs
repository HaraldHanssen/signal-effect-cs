namespace SignalEffect;

internal class Write<T> : Read<T>, IWrite<T>
where T : notnull
{
    internal Write(NodeId id, SignalNode<T> self, Func<T> get, Action<T> set) : base(id, self, get)
    {
        Set = set;
    }
    public Action<T> Set { get; }

    public Action<object> SetValue => (x) => Set((T)x);
}
