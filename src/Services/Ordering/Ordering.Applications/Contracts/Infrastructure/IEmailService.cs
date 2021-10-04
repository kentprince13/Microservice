using System.Threading.Tasks;
using Ordering.Applications.Models;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
