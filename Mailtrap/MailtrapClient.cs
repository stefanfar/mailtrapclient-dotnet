using Mailtrap.Entities;
using Mailtrap.Validators;
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
        private readonly HttpClient _httpClient;
        private readonly MailtrapOptions _mailtrapOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private MailtrapClient(HttpClient httpClient, MailtrapOptions mailtrapOptions, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mailtrapOptions = mailtrapOptions ?? throw new ArgumentNullException(nameof(mailtrapOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrEmpty(_mailtrapOptions.Token))
            {
                throw new ArgumentException(Constants.TokenIsRequired);
            }

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

        public MailtrapClient(string token) : this(new HttpClient(), new MailtrapOptions { Token = token }, new NullLogger<MailtrapClient>())
        {
        }

        public MailtrapClient(string token, ILogger<MailtrapClient> logger) : this(new HttpClient(), new MailtrapOptions { Token = token }, logger)
        {
        }

        public MailtrapClient(MailtrapOptions mailtrapOptions) : this(new HttpClient(), mailtrapOptions, new NullLogger<MailtrapClient>())
        {
        }

        public MailtrapClient(MailtrapOptions mailtrapOptions, ILogger<MailtrapClient> logger) : this(new HttpClient(), mailtrapOptions, logger)
        {
        }

        public MailtrapClient(HttpClient httpClient, IOptions<MailtrapOptions> mailtrapOptions, ILogger<MailtrapClient> logger) : this(httpClient, mailtrapOptions.Value, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<MailResponse> SendAsync(Mail mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException(nameof(mail));
            }

            if (!MailtrapValidator.TryValidateMail(mail, out var validationResults))
            {
                throw new ArgumentException(string.Join(Environment.NewLine, validationResults), nameof(mail));
            }

            var serializedMail = JsonSerializer.Serialize(mail, _serializationOptions);

            var jsonContent = new StringContent(serializedMail)
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

                return mailResponse;
            }
        }
    }
}
