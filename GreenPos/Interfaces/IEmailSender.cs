using GreenPOS.Models;
using System.Threading.Tasks;

namespace GreenPOS.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(QuotesViewModel quoteModel);
    }
}