using Mailtrap.Entities;

namespace Mailtrap
{
    public sealed class Constants
    {
        public static string SendingEndpoint = "https://send.api.mailtrap.io/api/send";
        public static string TokenIsRequired = $"The token is required.";
        public static string ToIsRequired = $"The property {nameof(Mail.To)} is required.";
        public static string FromIsRequired = $"The property {nameof(Mail.From)} is required.";
        public static string TextIsRequiredWhenHtmlIsMissing = $"The property {nameof(Mail.Text)} is required in the absence of {nameof(Mail.Html)}.";
        public static string TextShouldNotBeSetWhenHtmlIsSet = $"The property {nameof(Mail.Text)} should not be set because the property {nameof(Mail.Html)} is set.";
        public static string HtmlIsRequiredWhenTextIsMissing = $"The property {nameof(Mail.Html)} is required in the absence of {nameof(Mail.Text)}.";
        public static string AttachmentContentIsRequired = $"The property {nameof(Attachment.Content)} is required.";
        public static string AttachmentFilenameIsRequired = $"The property {nameof(Attachment.Filename)} is required.";
        public static string NoResponseReceived = "No response message was received from the server.";
        public static string MessageSuccesfullySent = "The message was succesfully sent to server.";
    }
}
