using Mailtrap.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mailtrap
{
    public class MailtrapClient : IMailtrapClient
    {
        private readonly MailtrapOptions _mailtrapOptions;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private MailtrapClient(HttpClient httpClient, MailtrapOptions mailtrapOptions, ILogger logger)
        {
            _httpClient = httpClient;
            _mailtrapOptions = mailtrapOptions;
            _logger = logger;

            if (mailtrapOptions.Timeout != default)
            {
                _httpClient.Timeout = mailtrapOptions.Timeout;
            }

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            if (mailtrapOptions.AuthorizationType == AuthorizationType.BearerAuth)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {mailtrapOptions.Token}");
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Add("Api-Token", mailtrapOptions.Token);
            }
        }

        public MailtrapClient(HttpClient httpClient, IOptions<MailtrapOptions> mailtrapOptions, ILogger<MailtrapClient> logger) : this(httpClient, mailtrapOptions.Value, logger)
        {
        }

        public MailtrapClient(string token, ILogger<MailtrapClient> logger) : this(new HttpClient(), new MailtrapOptions { Token = token }, logger)
        {
        }
        public MailtrapClient(string token) : this(new HttpClient(), new MailtrapOptions { Token = token }, new NullLogger<MailtrapClient>())
        {
        }

        public MailtrapClient(MailtrapOptions mailtrapOptions, ILogger<MailtrapClient> logger) : this(new HttpClient(), mailtrapOptions, logger)
        {
        }

        public async Task<MailResponse> SendAsync(Mail mail)
        {
            var serializedMail = JsonSerializer.Serialize(mail, _serializationOptions);

            StringContent jsonContent = new StringContent(serializedMail)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            };

            using (var httpResponseMessage = await _httpClient.PostAsync(_mailtrapOptions.SendingEnpoint ?? Constants.SendingEndpoint, jsonContent))
            {
                _logger.LogInformation(Constants.MessageSuccesfullySent);

                var mailResponseString = await httpResponseMessage.Content.ReadAsStringAsync();
                var mailResponse = JsonSerializer.Deserialize<MailResponse>(mailResponseString);

                mailResponse = mailResponse ?? new MailResponse() { Success = false, Errors = new List<string> { Constants.NoResponseReceived } };
                mailResponse.StatusCode = httpResponseMessage.StatusCode;

                Console.WriteLine($"{mailResponseString}\n");

                return mailResponse;
            }

        }
    }
}
