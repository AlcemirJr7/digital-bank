using Core.Extensions;

namespace Core.ApiResults;

public class ApiResultModel
{
    public object? Data { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public int StatusCode { get; set; }
    public ErrorDetails? Error { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now.Br();
}
