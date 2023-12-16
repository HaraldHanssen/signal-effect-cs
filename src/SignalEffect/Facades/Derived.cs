namespace SignalEffect;

internal class Derived<T> : Read<T>, IDerived<T>
where T: notnull
{
    internal Derived(NodeId id, DerivedNode<T> self, Func<T> get) : base(id, self, get)
    {
    }
}
