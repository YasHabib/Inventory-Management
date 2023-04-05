using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSQL;
using IMS.UseCases.Products.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMG.Plugins.EFCoreSQL
{
    public class ProductEFCoreRepository:IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> _contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Products.Where(i => i.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).Include(i => i.ProductInventories).ThenInclude(i => i.Inventory).ToListAsync();

        }

        public async Task AddProductAsync(Product product)
        {
            using var _context = _contextFactory.CreateDbContext();
            _context.Products.Add(product);

            CheckInventoryUnchanged(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var _context = _contextFactory.CreateDbContext();
            var currentProd = await _context.Products.Include(i => i.ProductInventories).ThenInclude(i => i.Inventory).FirstOrDefaultAsync(i => i.ProductId == product.ProductId);
            if(currentProd != null) 
            {
                currentProd.ProductName = product.ProductName;
                currentProd.Quantity = product.Quantity;
                currentProd.Price = product.Price;
                currentProd.ProductInventories = product.ProductInventories;
                CheckInventoryUnchanged(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product?> GetProductByIdAsync(int prodId)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Products.Include(i => i.ProductInventories).ThenInclude(i => i.Inventory).FirstOrDefaultAsync(i => i.ProductId == prodId);
        }

        private void CheckInventoryUnchanged(Product product)
        {
            using var _context = _contextFactory.CreateDbContext();
            if (product?.ProductInventories != null && product.ProductInventories.Count > 0)
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    if (prodInv.Inventory != null)
                    {
                        _context.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
