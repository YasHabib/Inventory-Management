﻿using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.Interfaces
{
    public interface IEditInventoryUseCase
    {
        Task EditExecuteAsync(Inventory inventory);
    }
}