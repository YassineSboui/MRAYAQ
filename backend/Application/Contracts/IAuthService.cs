using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Contracts;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
