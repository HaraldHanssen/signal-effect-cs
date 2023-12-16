namespace SignalEffect;

public static class ScopeExtensions
{
    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T>(this Scope s, Func<T> calc)
    where T : notnull
    {
        return s.Derived(calc);
    }

    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T, R>(this Scope s, IRead<R>[] r, Func<R[], T> calc)
    where T : notnull
    {
        return s.Derived(r, calc);
    }

    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T, R>(this Scope s, IRead<R> r, Func<R, T> calc)
    where T : notnull
    {
        return s.Derived(r, calc);
    }

    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T, R1, R2>(this Scope s, IRead<R1> r1, IRead<R2> r2, Func<R1, R2, T> calc)
    where T : notnull
    {
        return s.Derived(r1, r2, calc);
    }

    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T, R1, R2, R3>(this Scope s, IRead<R1> r1, IRead<R2> r2, IRead<R3> r3, Func<R1, R2, R3, T> calc)
    where T : notnull
    {
        return s.Derived(r1, r2, r3, calc);
    }

    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T, R1, R2, R3, R4>(this Scope s, IRead<R1> r1, IRead<R2> r2, IRead<R3> r3, IRead<R4> r4, Func<R1, R2, R3, R4, T> calc)
    where T : notnull
    {
        return s.Derived(r1, r2, r3, r4, calc);
    }

    /// <inheritdoc cref="Scope.Derived{T}(Func{T})"/>
    public static IDerived<T> Computed<T, R1, R2, R3, R4, R5>(this Scope s, IRead<R1> r1, IRead<R2> r2, IRead<R3> r3, IRead<R4> r4, IRead<R5> r5, Func<R1, R2, R3, R4, R5, T> calc)
    where T : notnull
    {
        return s.Derived(r1, r2, r3, r4, r5, calc);
    }
}