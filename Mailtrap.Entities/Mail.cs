using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mailtrap
{
    public class Mail
    {
        [JsonPropertyName("to")]
        [Required]
        public ICollection<Address> To { get; set; }

        [JsonPropertyName("from")]
        [Required(ErrorMessage = "Property From is mandatory")]
        public Address From { get; set; }

        [JsonPropertyName("subject")]
        [Required(ErrorMessage = "Property Subject is mandatory")]
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
