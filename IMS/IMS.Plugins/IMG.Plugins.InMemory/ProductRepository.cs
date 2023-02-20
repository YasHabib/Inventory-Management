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

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var lastId = _products.OrderByDescending(x => x.ProductId).Select(x => x.ProductId).FirstOrDefault();
            product.ProductId = lastId + 1;
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateProductAsync(Product inventory)
        {
            if (_products.Any(x => x.ProductId == inventory.ProductId && x.ProductName.Equals(inventory.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var inv = _products.FirstOrDefault(x => x.ProductId == inventory.ProductId);
            if (inv != null)
            {
                inv.ProductName = inventory.ProductName;
                inv.Quantity = inventory.Quantity;
                inv.Price = inventory.Price;
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await Task.FromResult(_products);
            else
                return _products.Where(i => i.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<Product> GetProductByIdAsync(int invId)
        {
            var inv = _products.FirstOrDefault(x => x.ProductId == invId);
            var newInv = new Product
            {
                ProductId = inv.ProductId,
                ProductName = inv.ProductName,
                Quantity = inv.Quantity,
                Price = inv.Price
            };

            return await Task.FromResult(newInv);
        }
    }
}
