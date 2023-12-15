namespace SignalEffect;

internal abstract class Primitive : IPrimitive
{
    internal Primitive(NodeId id, Node self)
    {
        Id = id;
        Self = self;
    }
    public NodeId Id { get; }
    internal Node Self { get; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return Id.Equals((obj as Primitive)?.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
