using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
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
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }


        public List<InventoryTransaction> inventoryTransactions = new List<InventoryTransaction>();

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateForm, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            var inventories = (await _inventoryRepository.GetInventoriesNameByAsync(string.Empty)).ToList();

            var query = from invtrans in inventoryTransactions
                        join inv in inventories on invtrans.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
                            (!dateForm.HasValue || invtrans.TransactionDate >= dateForm.Value.Date) &&
                            (!dateTo.HasValue || invtrans.TransactionDate <= dateTo.Value.Date) &&
                            (!transactionType.HasValue || invtrans.ActivityType == transactionType)
                        select new InventoryTransaction
                        {
                            Inventories = inv,
                            InventoryTransactionId = invtrans.InventoryTransactionId,
                            PurchaseOrderNumber = invtrans.PurchaseOrderNumber,
                            ProductionNumber = invtrans.ProductionNumber,
                            InventoryId = invtrans.InventoryId,
                            QuantityBefore = invtrans.QuantityBefore,
                            QuantityAfter = invtrans.QuantityAfter,
                            ActivityType = invtrans.ActivityType,
                            TransactionDate = invtrans.TransactionDate,
                            PurchasedBy = invtrans.PurchasedBy,
                            UnitPrice = invtrans.UnitPrice,
                        };
            return query;
        }

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
