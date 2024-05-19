
using System.ComponentModel.DataAnnotations;

namespace Mailtrap
{
    public interface IMailtrapClient
    {
        Task<MailResponse?> SendAsync(Mail mail);
    }
}