using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMG.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        public List<InventoryTransaction> inventoryTransactions = new List<InventoryTransaction>();

        public void ProduceAsync(string produceNumber, Inventory inventory, int quantity, string purchasedBy, double price)
        {
            inventoryTransactions.Add(new InventoryTransaction
            {
                ProductionNumber = produceNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.ProduceProduct,
                QuantityAfter = inventory.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                PurchasedBy = purchasedBy,
                UnitPrice = price

            });
        }

        public void PurchaseAsync(string purchaseOrderNumber, Inventory inventory, int quantity, string purchasedBy, double price)
        {
            inventoryTransactions.Add(new InventoryTransaction
            {
                PurchaseOrderNumber = purchaseOrderNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                PurchasedBy = purchasedBy,
                UnitPrice = price

            });
        }
    }
}
