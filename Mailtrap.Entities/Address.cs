using System.Text.Json.Serialization;

namespace Mailtrap
{
    public class Address
    {

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
