using IMS.CoreBusiness;
using IMS.UseCases.Products.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMG.Plugins.InMemory
{
    public class ProductRepository:IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product{ ProductId = 1, ProductName = "Bicycle", Quantity = 12, Price = 200},
                new Product{ ProductId = 2, ProductName = "Honda Civic", Quantity = 4, Price = 30000},
            };
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await Task.FromResult(_products);
            else
                return _products.Where(i => i.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _products.Max(x => x.ProductId);
            product.ProductId = maxId + 1;
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductId == product.ProductId && x.ProductName.ToLower() == product.ProductName))
            {
                return Task.CompletedTask;
            }

            var prod = _products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Quantity = product.Quantity;
                prod.Price = product.Price;
                prod.ProductInventories = product.ProductInventories;
            }

            return Task.CompletedTask;
        }

        public async Task<Product?> GetProductByIdAsync(int prodId)
        {
            var prod = _products.FirstOrDefault(x => x.ProductId == prodId);
            var newProd = new Product();
            if(prod != null)
            {
                newProd.ProductId = prod.ProductId;
                newProd.ProductName = prod.ProductName;
                newProd.Price = prod.Price;
                newProd.Quantity = prod.Quantity;
                newProd.ProductInventories = new List<ProductInventory>();
                if(prod.ProductInventories != null && prod.ProductInventories.Count > 0)
                {
                    foreach(var prodInv in prod.ProductInventories)
                    {
                        var newProdInv = new ProductInventory
                        {
                            ProductId = prodInv.ProductId,
                            InventoryId = prodInv.InventoryId,
                            Product = prod,
                            Inventory = new Inventory(),
                            InventoryQuantity = prodInv.InventoryQuantity
                        };
                        if(prodInv.Inventory != null)
                        {
                            newProdInv.Inventory.InventoryId = prodInv.Inventory.InventoryId;
                            newProdInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
                            newProdInv.Inventory.Price = prodInv.Inventory.Price;
                            newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                        }

                        newProd.ProductInventories.Add(newProdInv);
                    }
                }

            };

            return await Task.FromResult(newProd);
        }
    }
}
