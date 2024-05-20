﻿using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Mailtrap.Entities
{
    public class MailResponse
    {

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message_ids")]
        public ICollection<string> MessageIds { get; set; }

        [JsonPropertyName("errors")]
        public ICollection<string> Errors { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
