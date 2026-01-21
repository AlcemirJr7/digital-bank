using Core.Abstractions;
using Core.ApiResults;
using Core.Security.Errors;

namespace Core.Exceptions;

public class UnauthorizedApiException : Exception, IExceptionApi<ErrorDetails>
{
    public ErrorDetails ToError() => AuthErrors.Login.Invalid;
}