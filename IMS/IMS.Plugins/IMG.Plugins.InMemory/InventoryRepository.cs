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
                new Inventory{ InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 20},
                new Inventory{ InventoryId = 2, InventoryName = "Bike Body", Quantity = 13, Price = 100},
                new Inventory{ InventoryId = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 70},
                new Inventory{ InventoryId = 4, InventoryName = "Bike Padels", Quantity = 30, Price = 10},

            };
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if(_inventories.Any(x=> x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase))){
                return Task.CompletedTask;
            }

            var lastId = _inventories.OrderByDescending(x => x.InventoryId).Select(x => x.InventoryId).FirstOrDefault();
            inventory.InventoryId = lastId + 1;
            _inventories.Add(inventory);
            return Task.CompletedTask;
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            if(_inventories.Any(x => x.InventoryId == inventory.InventoryId && x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var inv = _inventories.FirstOrDefault(x => x.InventoryId == inventory.InventoryId);
            if (inv != null)
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Quantity = inventory.Quantity;
                inv.Price = inventory.Price;    
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesNameByAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await Task.FromResult(_inventories);
            else
                return _inventories.Where(i => i.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int invId)
        {
            var inv = _inventories.FirstOrDefault(x => x.InventoryId == invId);
            var newInv = new Inventory
            {
                InventoryId = inv.InventoryId,
                InventoryName = inv.InventoryName,
                Quantity = inv.Quantity,
                Price = inv.Price
            };

            return await Task.FromResult(newInv);
        }
    }
}