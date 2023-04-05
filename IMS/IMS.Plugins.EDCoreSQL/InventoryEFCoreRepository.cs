using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSQL;
using IMS.UseCases.Inventories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMG.Plugins.EFCoreSQL
{
    public class InventoryEFCoreRepository : IInventoryRepository
    {
        private readonly IDbContextFactory<IMSContext> _contextFactory;

        public InventoryEFCoreRepository(IDbContextFactory<IMSContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            using var _context = _contextFactory.CreateDbContext();
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            using var _context = _contextFactory.CreateDbContext();
            var currentInv = await _context.Inventories.FindAsync(inventory.InventoryId);
            if (currentInv != null)
            {
                currentInv.InventoryName = inventory.InventoryName;
                currentInv.Price = inventory.Price;
                currentInv.Quantity = inventory.Quantity;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesNameByAsync(string name)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Inventories.Where(i => i.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int invId)
        {
            using var _context = _contextFactory.CreateDbContext();
            var inv = await _context.Inventories.FindAsync(invId);
            if (inv != null)
                return inv;

            return new Inventory();
        }
    }
}