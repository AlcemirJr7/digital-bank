using Core.ApiResults;

namespace Core.Security.Auth;

public interface IAuthService
{
    ApiResult Autentica(AutenticaInputModel input);
}
