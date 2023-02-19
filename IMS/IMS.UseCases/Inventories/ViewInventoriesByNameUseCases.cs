using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Inventories
{
    public class ViewInventoriesByNameUseCases : IViewInventoriesByNameUseCases
    {
        private readonly IInventoryRepository inventoryRepository;
        public ViewInventoriesByNameUseCases(IInventoryRepository _inventoryRepository)
        {
            inventoryRepository = _inventoryRepository;
        }
        public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
        {
            return await inventoryRepository.GetInventoriesByNameAsync(name);
        }
    }
}
