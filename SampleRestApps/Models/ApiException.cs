using System.Net;

namespace SampleRestApps.Models;

public class ApiException : Exception
{
    public ApiException(string message, HttpStatusCode statusCode, string? response = null, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        Response = response;
    }

    public HttpStatusCode StatusCode { get; }

    public string? Response { get; }
}
