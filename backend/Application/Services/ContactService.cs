using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactResult> HandleAsync(ContactRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Message))
        {
            return ContactResult.Fail("Name and message are required.");
        }

        var message = new ContactMessage
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Message = request.Message.Trim(),
            Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(message, cancellationToken);

        var response = new ContactResponse(
            "Merci! We received your message.",
            DateTimeOffset.UtcNow
        );

        return ContactResult.Ok(response);
    }
}
