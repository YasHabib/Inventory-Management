using IMS.CoreBusiness;

namespace IMS.UseCases.Actvities.Interfaces
{
    public interface IPurchaseInventoryUseCase
    {
        Task ExecuteAsync(string postalCode, Inventory inventory, int quantity, string purchasedBy);
    }
}