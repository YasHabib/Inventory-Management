﻿using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Inventories.Interfaces
{
    public interface IInventoryRepository
    {
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task<IEnumerable<Inventory>> GetInventoriesNameByAsync(string name);
        Task<Inventory> GetInventoryByIdAsync(int invId);
    }
}
