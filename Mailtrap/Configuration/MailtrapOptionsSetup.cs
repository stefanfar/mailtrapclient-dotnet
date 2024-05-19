using Mailtrap.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Mailtrap.Configuration
{
    public class MailtrapOptionsSetup : IConfigureOptions<MailtrapOptions>
    {
        private const string SectionName = "Mailtrap";
        private readonly IConfiguration _configuration;

        public MailtrapOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MailtrapOptions options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
