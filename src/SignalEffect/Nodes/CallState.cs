namespace SignalEffect;

internal class CallState(List<IValueNode>? dependencies, bool noCall, bool noWrite) {
    public List<IValueNode>? Dependencies { get; } = dependencies;
    public bool NoCall { get; } = noCall;
    public bool NoWrite { get; } = noWrite;
}
