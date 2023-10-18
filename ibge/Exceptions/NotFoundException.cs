namespace ibge.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not Found") { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public static NotFoundException Create(string message = "Not Found")
        {
            return new NotFoundException(message);
        }
    }
}