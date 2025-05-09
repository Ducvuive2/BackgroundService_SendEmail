using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPurchasingManagerRepository
    {
        Task<IEnumerable<PurchasingManager>> GetAllPurchasingManagersAsync();
    }
} 