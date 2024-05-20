using System.Text.Json.Serialization;

namespace Mailtrap.Entities
{
    public class Attachment
    {
        [JsonPropertyName("content")]
        [JsonRequired]
        public string Content { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("filename")]
        [JsonRequired]
        public string Filename { get; set; }

        [JsonPropertyName("disposition")]
        public string Disposition { get; set; }

        [JsonPropertyName("content_id")]
        public string ContentId { get; set; }
    }
}
