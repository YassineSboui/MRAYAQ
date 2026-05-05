using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAdminRepository _admins;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public AuthService(IAdminRepository admins, IPasswordHasher hasher, ITokenService tokens)
    {
        _admins = admins;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _admins.GetByUsernameAsync(request.Username.Trim(), cancellationToken);
        if (user is null)
        {
            return null;
        }

        var valid = _hasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
        if (!valid)
        {
            return null;
        }

        var token = _tokens.CreateToken(user);
        return new LoginResponse(token, user.Username);
    }
}
