namespace ibge.Exceptions;

public class ConflictException : Exception
{
    public ConflictException() : base("Conflict")
    {
    }

    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static ConflictException Create(string message = "Conflict")
    {
        return new ConflictException(message);
    }
}