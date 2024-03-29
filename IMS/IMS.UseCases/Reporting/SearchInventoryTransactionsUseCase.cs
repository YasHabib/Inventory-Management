﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reporting.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reporting
{
    public class SearchInventoryTransactionsUseCase : ISearchInventoryTransactionsUseCase
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        public SearchInventoryTransactionsUseCase(IInventoryTransactionRepository inventoryTransactionRepository)
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? dateForm, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            return await _inventoryTransactionRepository.GetInventoryTransactionsAsync(inventoryName, dateForm, dateTo, transactionType);
        }
    }
}
