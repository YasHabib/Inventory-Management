using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSQL;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMG.Plugins.EFCoreSQL
{
    public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepository
    {
        private readonly IDbContextFactory<IMSContext> _contextFactory;

        public InventoryTransactionEFCoreRepository(IDbContextFactory<IMSContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }


        public List<InventoryTransaction> inventoryTransactions = new List<InventoryTransaction>();

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateForm, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            using var _context = _contextFactory.CreateDbContext();

            var query = from invtrans in _context.InventoryTransactions
                        join inv in _context.Inventories on invtrans.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
                            (!dateForm.HasValue || invtrans.TransactionDate >= dateForm.Value.Date) &&
                            (!dateTo.HasValue || invtrans.TransactionDate <= dateTo.Value.Date) &&
                            (!transactionType.HasValue || invtrans.ActivityType == transactionType)
                        select invtrans;
            return await query.Include(i => i.Inventories).ToListAsync();
        }

        public async Task ProduceAsync(string produceNumber, Inventory inventory, int quantity, string purchasedBy, double price)
        {
            using var _context = _contextFactory.CreateDbContext();

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductionNumber = produceNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                PurchasedBy = purchasedBy,
                UnitPrice = price

            });

            await _context.SaveChangesAsync();
        }

        public async Task PurchaseAsync(string purchaseOrderNumber, Inventory inventory, int quantity, string purchasedBy, double price)
        {
            using var _context = _contextFactory.CreateDbContext();

            _context.InventoryTransactions.Add(new InventoryTransaction
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

            await _context.SaveChangesAsync();  
        }
    }
}
