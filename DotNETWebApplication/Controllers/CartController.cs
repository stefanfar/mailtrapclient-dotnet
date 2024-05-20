using Mailtrap;
using Mailtrap.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly IMailtrapClient _mailtrapClient;

        public CartController(ILogger<CartController> logger, IMailtrapClient mailtrapClient)
        {
            _logger = logger;
            _mailtrapClient = mailtrapClient;
        }

        [HttpPost]
        public async Task Pay()
        {
            var mail = new Mail
            {
                To = new List<Address> { new Address { Email = "john_doe@example.com", Name = "John Doe" } },
                From = new Address { Email = "sales@example.com", Name = "Example Sales Team" },
                Subject = "Your Example Order Confirmation",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test"
            };

            try
            {
                await _mailtrapClient.SendAsync(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sending the email failed");
            }
        }
    }
}
