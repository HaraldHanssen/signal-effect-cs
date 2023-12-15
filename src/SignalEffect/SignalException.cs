namespace SignalEffect;

public abstract class SignalException : Exception
{
    protected SignalException(string? message) : base(message)
    {
    }
}
