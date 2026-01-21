using Core.ApiResults;
using MediatR;

namespace ContaCorrente.Application.Security.Login;

public sealed class LoginRequest : IRequest<ApiResult<LoginResponse>>
{
    public int? NumeroConta { get; set; } = default;
    public string? Documento { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
