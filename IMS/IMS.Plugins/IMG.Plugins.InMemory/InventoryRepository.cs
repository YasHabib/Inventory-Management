using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;

namespace IMG.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories = new List<Inventory>()
            {
                new Inventory{ InventoryId = 1, ProductName = "Bike Seat", Quantity = 10, Price = 20},
                new Inventory{ InventoryId = 2, ProductName = "Bike Body", Quantity = 13, Price = 100},
                new Inventory{ InventoryId = 3, ProductName = "Bike Wheels", Quantity = 20, Price = 70},
                new Inventory{ InventoryId = 4, ProductName = "Bike Padels", Quantity = 30, Price = 10},

            };
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if(_inventories.Any(x=> x.ProductName.Equals(inventory.ProductName, StringComparison.OrdinalIgnoreCase))){
                return Task.CompletedTask;
            }

            var lastId = _inventories.OrderByDescending(x => x.InventoryId).Select(x => x.InventoryId).FirstOrDefault();
            inventory.InventoryId = lastId + 1;
            _inventories.Add(inventory);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await Task.FromResult(_inventories);
            else
                return _inventories.Where(i => i.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}