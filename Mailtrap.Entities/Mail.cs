using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mailtrap.Entities
{
    public class Mail
    {
        [JsonPropertyName("to")]
        [JsonRequired]
        public ICollection<Address> To { get; set; }

        [JsonPropertyName("from")]
        [JsonRequired]
        public Address From { get; set; }

        [JsonPropertyName("subject")]
        [JsonRequired]
        public string Subject { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("html")]
        public string Html { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("attachments")]
        public ICollection<Attachment> Attachments { get; set; }
    }
}
