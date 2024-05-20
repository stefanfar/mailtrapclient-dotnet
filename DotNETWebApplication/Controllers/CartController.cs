using Mailtrap;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CartController> _logger;
        private readonly IMailtrapClient _mailtrapClient;

        public CartController(ILogger<CartController> logger, IMailtrapClient mailtrapClient)
        {
            _logger = logger;
            _mailtrapClient = mailtrapClient;
        }

        [HttpPost]
        public void Pay()
        {
            var mailparams = new Mail
            {
                To = new List<Address> { new Address { Email = "4plx4.test@inbox.testmail.app", Name = "abc" } },
                From = new Address { Email = "stefan.farcas9@gmail.com", Name = "abc" },
                Subject = "subiect",
                //Text = "un text",
                Html = "<!doctype html><p>a</p>",
                Category = "API test"
            };


            _mailtrapClient.SendAsync(mailparams).Wait();
        }
    }
}
