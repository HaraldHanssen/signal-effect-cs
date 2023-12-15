namespace SignalEffect;

internal class DerivedSignal<T> : ReadonlySignal<T>
where T: notnull
{
    internal DerivedSignal(NodeId id, DerivedNode<T> self, Func<T> get) : base(id, self, get)
    {
    }
}
