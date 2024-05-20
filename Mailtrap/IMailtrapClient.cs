using System.Threading.Tasks;

namespace Mailtrap
{
    public interface IMailtrapClient
    {
        Task<MailResponse> SendAsync(Mail mail);
    }
}