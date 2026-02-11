namespace ColinhoDaCa.Domain._Shared.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(string message) : base(message)
    {
    }

    public override int HttpStatusCode => 400;
}
