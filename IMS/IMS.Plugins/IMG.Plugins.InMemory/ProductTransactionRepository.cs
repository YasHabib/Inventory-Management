using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interface;
using System;
using System.Collections.Generic;
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
                        _inventoryTransactionRepository.ProduceAsync(productionNumber, pi.Inventory, pi.InventoryQuantity * quantity, completedBy, -1);

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
    }
}
