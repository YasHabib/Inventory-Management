using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        Task PurchaseAsync(string postalCode, Inventory inventory, int quantity, string purchasedBy, double price);
        Task ProduceAsync(string productioneNumber, Inventory inventory, int quantity, string purchasedBy, double price);
        Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateForm, DateTime? dateTo, InventoryTransactionType? transactionType);
    }
}