namespace ColinhoDaCa.Domain._Shared.Exceptions;

public interface IHasHttpCode
{
    public int HttpStatusCode { get; }
}