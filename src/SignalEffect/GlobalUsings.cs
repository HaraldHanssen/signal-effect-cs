global using NodeId = uint;
global using SequenceNumber = ulong;
global using InMap = System.Collections.Generic.Dictionary<uint, SignalEffect.IValueNode>;
global using OutMap = System.Collections.Generic.Dictionary<uint, System.WeakReference<SignalEffect.DependentNode>>;