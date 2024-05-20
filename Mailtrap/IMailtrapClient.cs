using Mailtrap.Entities;
using System.Threading.Tasks;

namespace Mailtrap
{
    public interface IMailtrapClient
    {
        /// <summary>
        /// Sends email asynchronously
        /// </summary>
        /// <param name="mail"></param>
        /// <returns>Email response, indicating if the email was delivered successfully</returns>
        Task<MailResponse> SendAsync(Mail mail);
    }
}