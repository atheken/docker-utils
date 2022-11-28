namespace app;

/// <summary>
/// A fully parsed email address.
/// </summary>
/// <param name="Email"></param>
/// <param name="Name"></param>
/// <param name="MailboxHash"></param>
public record EmailAddress(string Email, String Name, String MailboxHash);

/// <summary>
/// A parsed Inbound message payload.
/// </summary>
/// <param name="FromFull"></param>
/// <param name="ToFull"></param>
/// <param name="Subject"></param>
/// <param name="HtmlBody"></param>
/// <param name="TextBody"></param>
/// <param name="MessageID"></param>
public record InboundPayload(EmailAddress FromFull, EmailAddress ToFull,
    String Subject, String HtmlBody, String TextBody, String MessageID);