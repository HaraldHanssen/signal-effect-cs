namespace SignalEffect;

public interface IDerived : IRead {

}

public interface IDerived<T> : IDerived, IRead<T> {
}