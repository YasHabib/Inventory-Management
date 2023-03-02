using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        void PurchaseAsync(string postalCode, Inventory inventory, int quantity, string purchasedBy, double price);
        void ProduceAsync(string productioneNumber, Inventory inventory, int quantity, string purchasedBy, double price);
        Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateForm, DateTime? dateTo, InventoryTransactionType? transactionType);
    }
}