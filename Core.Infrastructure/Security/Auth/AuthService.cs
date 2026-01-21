using Core.ApiResults;
using Core.Security.Auth;
using Core.Security.Errors;

namespace Core.Infrastructure.Security.Auth;

public sealed class AuthService(ILogin login) : IAuthService
{
    public ApiResult Autentica(AutenticaInputModel input)
    {
        var isSenhaValida = login.ValidaSenha(input.Senha, input.Hash, input.Salt);

        if (!isSenhaValida)
            return ApiResult.Failure(AuthErrors.Login.InvalidPassword);

        return ApiResult.Success();
    }
}
