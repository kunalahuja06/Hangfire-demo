using HangFire.Data.Models;

namespace HangFire.Application.Contracts
{
    public interface IMailService
    {
        Task SendAsync ( MailRequest request );
    }
}
