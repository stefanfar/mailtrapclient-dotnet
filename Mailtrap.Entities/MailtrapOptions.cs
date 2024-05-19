namespace Mailtrap.Entities
{
    public class MailtrapOptions
    {
        public string? Token { get; set; }

        public AuthorizationType? AuthorizationType { get; set; }

        public string? SendingEnpoint { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}
