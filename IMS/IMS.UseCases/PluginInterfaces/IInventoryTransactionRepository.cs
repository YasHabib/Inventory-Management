using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        void PurchaseAsync(string postalCode, Inventory inventory, int quantity, string purchasedBy, double price);
    }
}