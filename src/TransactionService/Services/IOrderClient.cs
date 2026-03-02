using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Services
{
    public interface IOrderClient
    {
        Task<OrderDto?> GetOrderAsync(int orderId);
    }
}
