using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSQL;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMG.Plugins.EFCoreSQL
{
    public class ProductTransactionEFCoreRepository : IProductTransactionRepository
    {

        private readonly IDbContextFactory<IMSContext> _contextFactory;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductTransactionEFCoreRepository(
            IProductRepository productRepository, 
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository,
            IDbContextFactory<IMSContext> dbContextFactory)
        {
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
            _contextFactory = dbContextFactory;
        }
        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string completedBy)
        {
            using var _context = _contextFactory.CreateDbContext();
            var prod = await _productRepository.GetProductByIdAsync(product.ProductId);
            if(prod != null)
            {
                foreach(var pi in prod.ProductInventories)
                {
                    if(pi.Inventory != null)
                    {
                        //add inventory transaction
                        await _inventoryTransactionRepository.ProduceAsync(productionNumber, pi.Inventory, pi.InventoryQuantity * quantity, completedBy, -1);

                        //decerase the inventory
                        var inv = await _inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;
                        await this._inventoryRepository.UpdateInventoryAsync(inv);
                    }

                }
            }

            //add product transaction
            _context.ProductTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,  
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                CompletedBy = completedBy,
            });

            await _context.SaveChangesAsync();
        }

        public async Task SellProductsAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string completedBy)
        {
            using var _context = _contextFactory.CreateDbContext();

            _context.ProductTransactions.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SalesOrderNumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                CompletedBy = completedBy,
                UnitPrice = unitPrice,
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateForm, DateTime? dateTo, ProductTransactionType? transactionType)
        {

            using var _context = _contextFactory.CreateDbContext();
            var query = from prodtrans in _context.ProductTransactions
                        join prod in _context.Products on prodtrans.ProductId equals prod.ProductId
                        where
                            (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                            (!dateForm.HasValue || prodtrans.TransactionDate >= dateForm.Value.Date) &&
                            (!dateTo.HasValue || prodtrans.TransactionDate <= dateTo.Value.Date) &&
                            (!transactionType.HasValue || prodtrans.ActivityType == transactionType)
                        select prodtrans;
            return await query.Include(i => i.Product).ToListAsync();
        }
    }
}
