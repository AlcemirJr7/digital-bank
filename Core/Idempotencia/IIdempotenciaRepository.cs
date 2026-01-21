namespace Core.Idempotencia;

public interface IIdempotenciaRepository
{
    Task<IdempotenciaEntity?> GetAsync(string chaveIdempotencia);
    Task<bool> CreateAsync(IdempotenciaEntity entity);
    Task<bool> CleanUpAsync(int tempo);
}
