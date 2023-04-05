using IMS.CoreBusiness;
using IMS.UseCases.Actvities.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Actvities
{
    public class SellProductUseCase : ISellProductUseCase
    {
        private readonly IProductTransactionRepository _productTransactionRepository;
        private readonly IProductRepository _productRepository;
        public SellProductUseCase(IProductTransactionRepository productTransactionRepository, IProductRepository productRepository)
        {
            _productTransactionRepository = productTransactionRepository;
            _productRepository = productRepository;
        }
        public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double UnitPrice, string completedBy)
        {
            await _productTransactionRepository.SellProductsAsync(salesOrderNumber, product, quantity, UnitPrice, completedBy);
            product.Quantity -= quantity;
            await _productRepository.UpdateProductAsync(product);
        }
    }
}
