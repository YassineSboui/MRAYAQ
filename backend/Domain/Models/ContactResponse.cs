namespace MRAYAQ.Domain.Models;

public record ContactResponse(string Message, DateTimeOffset ReceivedAt);

public record ContactResult(bool Success, ContactResponse? Response, string? ErrorMessage)
{
    public static ContactResult Ok(ContactResponse response) => new(true, response, null);

    public static ContactResult Fail(string errorMessage) => new(false, null, errorMessage);
}
