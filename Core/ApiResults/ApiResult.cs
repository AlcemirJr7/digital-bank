using Core.Extensions;

namespace Core.ApiResults;

public class ApiResult
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public int StatusCode { get; init; }
    public ErrorDetails? Error { get; init; }
    public string Timestamp { get; init; } = DateTime.Now.BrStr();

    public ApiResult(bool isSuccess, int statusCode, ErrorDetails? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static ApiResult Success() =>
        new(true, ApiStatusCode.Ok);

    public static ApiResult SuccessNoContent() =>
        new(true, ApiStatusCode.NoContent);

    public static ApiResult<T> Success<T>(T data) =>
        new(data, true, ApiStatusCode.Ok);

    public static ApiResult<T> SuccessCreated<T>(T data) =>
        new(data, true, ApiStatusCode.Created);

    public static ApiResult Failure(ErrorDetails? error) =>
        new(false, error!.StatusCode, error);

    public static ApiResult<T> Failure<T>(ErrorDetails? error) =>
        new(default, false, error!.StatusCode, error);
}

public class ApiResult<T> : ApiResult
{
    public T Data { get; init; } = default!;

    public ApiResult(T? data, bool isSuccess, int statusCode, ErrorDetails? error = null)
        : base(isSuccess, statusCode, error)
    {
        Data = data!;
    }

    public static implicit operator ApiResult<T>(T? data) =>
        data is not null ? Success(data) : Failure<T>(default);
}