namespace app;

/// <summary>
/// A fully parsed email address.
/// </summary>
/// <param name="Email"></param>
/// <param name="Name"></param>
/// <param name="MailboxHash"></param>
public record EmailAddress(string? Email = null, string? Name = null, string? MailboxHash = null);

/// <summary>
/// A parsed Inbound message payload.
/// </summary>
/// <param name="FromFull"></param>
/// <param name="ToFull"></param>
/// <param name="Subject"></param>
/// <param name="HtmlBody"></param>
/// <param name="TextBody"></param>
/// <param name="MessageID"></param>
public record InboundPayload(EmailAddress? FromFull, EmailAddress[] ToFull,
    string? Subject = null, string? HtmlBody = null, string? TextBody = null, string? MessageId = null);

/// <summary>
/// A generic status message, ErrorCode is null when successful.
/// </summary>
/// <param name="Message"></param>
/// <param name="ErrorCode"></param>
public record Status(string Message, int? ErrorCode = null);

public record CreateRecipeRequest(string url);