using IMS.CoreBusiness;
using IMS.UseCases.Actvities.Interfaces;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Actvities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;
        public PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository, IInventoryRepository inventoryRepository)
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(string postalCode, Inventory inventory, int quantity, string purchasedBy)
        {
            //insert the record in the transaction table
            _inventoryTransactionRepository.PurchaseAsync(postalCode, inventory, quantity, purchasedBy, inventory.Price);

            //update the inventory
            inventory.Quantity += quantity;
            await _inventoryRepository.UpdateInventoryAsync(inventory);

        }
    }
}
