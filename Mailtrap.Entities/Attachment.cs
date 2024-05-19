using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mailtrap
{
    public class Attachment
    {
        [JsonPropertyName("content")]
        [Required(ErrorMessage = "Property Content is mandatory")]
        public string? Content { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("filename")]
        [Required(ErrorMessage = "Property Filename is mandatory")]
        public string? Filename { get; set; }

        [JsonPropertyName("disposition")]
        public string? Disposition { get; set; }

        [JsonPropertyName("content_id")]
        public string? ContentId { get; set; }
    }
}
