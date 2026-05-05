using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Contracts;

public interface IContactService
{
    Task<ContactResult> HandleAsync(ContactRequest request, CancellationToken cancellationToken = default);
}
