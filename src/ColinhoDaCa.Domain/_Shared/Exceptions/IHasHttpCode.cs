using System.Net;

namespace ColinhoDaCa.Domain._Shared.Exceptions;

public interface IHasHttpCode
{
    public HttpStatusCode StatusCode { get; }
}