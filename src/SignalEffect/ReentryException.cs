namespace SignalEffect;

public class ReentryException : SignalException
{
    public const string ERR_CALL = "Calling an effect within a derived/effect callback is not allowed.";
    public const string ERR_WRITE = "Writing to a signal within a derived callback is not allowed";
    public const string ERR_LOOP = "Recursive loop detected";
    public ReentryException(string? message) : base(message)
    {
    }
}
