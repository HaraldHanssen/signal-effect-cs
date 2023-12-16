namespace SignalEffect;

public class Scope : IDisposable
{
    public Scope(IExecution? handler = null)
    {
    }

    public void Dispose()
    {
    }

    /// <summary>
    /// Create a single writable signal with the provided initial value.
    /// </summary>
    public IWrite<T> Signal<T>(T initial)
    where T : notnull
    {
        return SignalNode<T>.Signal(initial);
    }



    /// <summary>
    /// Create an array of writable signals with the provided initial value.
    /// </summary>
    public IWrite<T>[] Signals<T>(params T[] initial)
    where T : notnull
    {
        return initial.Select(x => SignalNode<T>.Signal(x)).ToArray();
    }

    /// <summary>
    /// Modify internal properties or elements within a signal and set the result.
    /// Useful for large nested objects and array manipulation, where the cost of
    /// modification is great.
    /// </summary>
    public void Modify<T>(IWrite<T> signal, Action<T> manipulate)
    where T : notnull
    {
        SignalNode<T>.Modify(signal, manipulate);
    }

    /// <summary>
    /// Create a read only signal from an existing signal.
    /// </summary>
    public IRead<T> Readonly<T>(IWrite<T> signal)
    where T : notnull
    {
        return SignalNode<T>.Readonly(signal);
    }

    /// <summary>
    /// Create a derived/calculated signal from one or more sources.
    /// 
    /// To avoid side effects the calculation function is not allowed to reenter the signal system to:
    /// (a) set a value on a writable, or
    /// (b) execute an effect.
    /// Reading values from signals are allowed.
    /// </summary>
    public IDerived<T> Derived<T>(Func<T> calc)
    where T : notnull
    {
        return DynamicDerivedNode<T>.Derived(calc);
    }

    /// <inheritdoc cref="Derived{T}(Func{T})"/>
    public IDerived<T> Derived<T, R>(IRead<R>[] r, Func<R[], T> calc)
    where T : notnull
    {
        var ri = r.Select(x => x.ValueNode()).ToList();
        return FixedDerivedNode<T>.Derived(ri, (v) => calc(v.Select(x => (R)x).ToArray()));
    }

    /// <inheritdoc cref="Derived{T}(Func{T})"/>
    public IDerived<T> Derived<T, R>(IRead<R> r, Func<R, T> calc)
    where T : notnull
    {
        return FixedDerivedNode<T>.Derived([r.ValueNode()], (v) => calc((R)v[0]));
    }

    /// <inheritdoc cref="Derived{T}(Func{T})"/>
    public IDerived<T> Derived<T, R1, R2>(IRead<R1> r1, IRead<R2> r2, Func<R1, R2, T> calc)
    where T : notnull
    {
        return FixedDerivedNode<T>.Derived([r1.ValueNode(), r2.ValueNode()], (v) => calc((R1)v[0], (R2)v[1]));
    }

    /// <inheritdoc cref="Derived{T}(Func{T})"/>
    public IDerived<T> Derived<T, R1, R2, R3>(IRead<R1> r1, IRead<R2> r2, IRead<R3> r3, Func<R1, R2, R3, T> calc)
    where T : notnull
    {
        return FixedDerivedNode<T>.Derived([r1.ValueNode(), r2.ValueNode(), r3.ValueNode()], (v) => calc((R1)v[0], (R2)v[1], (R3)v[2]));
    }

    /// <inheritdoc cref="Derived{T}(Func{T})"/>
    public IDerived<T> Derived<T, R1, R2, R3, R4>(IRead<R1> r1, IRead<R2> r2, IRead<R3> r3, IRead<R4> r4, Func<R1, R2, R3, R4, T> calc)
    where T : notnull
    {
        return FixedDerivedNode<T>.Derived([r1.ValueNode(), r2.ValueNode(), r3.ValueNode(), r4.ValueNode()], (v) => calc((R1)v[0], (R2)v[1], (R3)v[2], (R4)v[3]));
    }

    /// <inheritdoc cref="Derived{T}(Func{T})"/>
    public IDerived<T> Derived<T, R1, R2, R3, R4, R5>(IRead<R1> r1, IRead<R2> r2, IRead<R3> r3, IRead<R4> r4, IRead<R5> r5, Func<R1, R2, R3, R4, R5, T> calc)
    where T : notnull
    {
        return FixedDerivedNode<T>.Derived([r1.ValueNode(), r2.ValueNode(), r3.ValueNode(), r4.ValueNode(), r5.ValueNode()], (v) => calc((R1)v[0], (R2)v[1], (R3)v[2], (R4)v[3], (R5)v[4]));
    }
}
