using Mailtrap.Entities;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;

namespace Mailtrap.Configuration
{
    public static class MailtrapConfiguration
    {

        public static void AddMailtrap(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync();

            services.ConfigureOptions<MailtrapOptionsSetup>();
            services.AddHttpClient<IMailtrapClient, MailtrapClient>().AddPolicyHandler(retryPolicy);
        }

        public static void AddMailtrap(this IServiceCollection services, Action<MailtrapOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync();

            services.Configure(options);
            services.AddHttpClient<IMailtrapClient, MailtrapClient>().AddPolicyHandler(retryPolicy);
        }
    }
}
