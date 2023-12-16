namespace SignalEffect;

internal interface ICallTrack
{
    CallState State { get; set; }

    void Add(Effect e);
    
    void Add<T>(Derived<T> d) where T : notnull;

    void Enter();

    void Exit();
}

internal class CallState(List<IValueNode>? dependencies, bool noCall, bool noWrite) {
    public List<IValueNode>? Dependencies { get; } = dependencies;
    public bool NoCall { get; } = noCall;
    public bool NoWrite { get; } = noWrite;
}
