namespace SignalEffect;

internal interface ICallTrack
{
    CallState State { get; set; }

    void Add(IEffect e);
    
    void Add<T>(IDerived<T> d) where T : notnull;

    void Changed(IRead read, List<IDerived> deriveds, List<IEffect> effects);
    
    void Enter();

    void Handle(DependentNode dependentNode, SequenceNumber current);

    void Exit();
}

internal class CallState(List<IValueNode>? dependencies, bool noCall, bool noWrite) {
    public List<IValueNode>? Dependencies { get; } = dependencies;
    public bool NoCall { get; } = noCall;
    public bool NoWrite { get; } = noWrite;
}
