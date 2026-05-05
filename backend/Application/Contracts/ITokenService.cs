using MRAYAQ.Domain.Entities;

namespace MRAYAQ.Application.Contracts;

public interface ITokenService
{
    string CreateToken(AdminUser admin);
}
