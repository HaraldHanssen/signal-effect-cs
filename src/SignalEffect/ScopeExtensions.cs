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

    /// <inheritdoc cref="ManualExecution.Update(IEnumerable{IDerived})"/>
    public static void Update(this Scope<ManualExecution> s, IEnumerable<IDerived> deriveds) {
        s.Handler.Update(deriveds);
    }

    /// <inheritdoc cref="ManualExecution.Update(IEnumerable{IEffect})"/>
    public static void Update(this Scope<ManualExecution> s, IEnumerable<IEffect> effects) {
        s.Handler.Update(effects);
    }

    public static void Deconstruct<T>(this T[] list, out T first, out T[] rest)
    {
        first = list.Length > 0 ? list[0] : throw new ArgumentOutOfRangeException(nameof(list));
        rest = list[1..];
    }

    public static void Deconstruct<T>(this T[] list, out T first, out T second, out T[] rest)
    {
        first = list.Length > 0 ? list[0] : throw new ArgumentOutOfRangeException(nameof(list));
        second = list.Length > 1 ? list[1] : throw new ArgumentOutOfRangeException(nameof(list));
        rest = list[2..];
    }

    public static void Deconstruct<T>(this T[] list, out T first, out T second, out T third, out T[] rest)
    {
        first = list.Length > 0 ? list[0] : throw new ArgumentOutOfRangeException(nameof(list));
        second = list.Length > 1 ? list[1] : throw new ArgumentOutOfRangeException(nameof(list));
        third = list.Length > 2 ? list[2] : throw new ArgumentOutOfRangeException(nameof(list));
        rest = list[3..];
    }
}