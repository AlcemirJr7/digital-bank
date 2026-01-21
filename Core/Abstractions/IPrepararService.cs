namespace Core.Abstractions;

public interface IPrepararService<in TRequest, TResult> 
    where TResult : class
    where TRequest : class
{
    Task<TResult> PrepararAsync(TRequest request, CancellationToken ct);
}
