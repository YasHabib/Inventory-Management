using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMG.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {

        private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();

        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductTransactionRepository(
            IProductRepository productRepository, 
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository
            )
        {
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
        }
        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string completedBy)
        {
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
            _productTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,  
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                CompletedBy = completedBy,
            });
        }

        public Task SellProductsAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string completedBy)
        {

            _productTransactions.Add(new ProductTransaction
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

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateForm, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            var products = (await _productRepository.GetProductsByNameAsync(string.Empty)).ToList();

            var query = from prodtrans in _productTransactions
                        join prod in products on prodtrans.ProductId equals prod.ProductId
                        where
                            (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                            (!dateForm.HasValue || prodtrans.TransactionDate >= dateForm.Value.Date) &&
                            (!dateTo.HasValue || prodtrans.TransactionDate <= dateTo.Value.Date) &&
                            (!transactionType.HasValue || prodtrans.ActivityType == transactionType)
                        select new ProductTransaction
                        {
                            Product = prod,
                            ProductTransactionId = prodtrans.ProductTransactionId,
                            SalesOrderNumber = prodtrans.SalesOrderNumber,
                            ProductionNumber = prodtrans.ProductionNumber,
                            ProductId = prodtrans.ProductId,
                            QuantityBefore = prodtrans.QuantityBefore,
                            QuantityAfter = prodtrans.QuantityAfter,
                            ActivityType = prodtrans.ActivityType,
                            TransactionDate = prodtrans.TransactionDate,
                            CompletedBy = prodtrans.CompletedBy,
                            UnitPrice = prodtrans.UnitPrice,
                        };
            return query;
        }
    }
}
