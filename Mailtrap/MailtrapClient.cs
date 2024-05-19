﻿using Mailtrap.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

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

            if (mailtrapOptions.Timeout.HasValue)
            {
                _httpClient.Timeout = mailtrapOptions.Timeout.Value;
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

        public async Task<MailResponse?> SendAsync(Mail mail)
        {
            var serializedMail = JsonSerializer.Serialize(mail, _serializationOptions);

            using StringContent jsonContent = new StringContent(serializedMail)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            };

            using HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(
                _mailtrapOptions.SendingEnpoint ?? Constants.SendingEndpoint,
                jsonContent);

            _logger.LogInformation(Constants.MessageSuccesfullySent);

            var mailResponse = await httpResponseMessage.Content.ReadFromJsonAsync<MailResponse>();
            mailResponse ??= new MailResponse() { Success = false, Errors = new List<string> { Constants.NoResponseReceived } };
            mailResponse.StatusCode = httpResponseMessage.StatusCode;

            var res = await httpResponseMessage.Content.ReadAsStringAsync();
            Console.WriteLine($"{res}\n");

            return mailResponse;
        }
    }
}