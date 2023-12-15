namespace SignalEffect;

internal class Effect : Primitive, IEffect {
    internal Effect(NodeId id, EffectNode self, Action action) : base(id, self)
    {
        Call = action;
    }

    public Action Call { get; }
}
