using System.Runtime.Serialization;

namespace ColinhoDaCa.Domain._Shared.Exceptions;

public abstract class BaseException : Exception, IHasHttpCode
{
    protected BaseException()
    {
    }

    protected BaseException(string message)
        : base(message)
    {
    }

    protected BaseException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected BaseException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }

    public abstract int HttpStatusCode { get; }
}