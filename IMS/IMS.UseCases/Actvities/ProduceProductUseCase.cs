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
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductTransactionRepository _productTransactionRepository;
        private readonly IProductRepository _productRepository;
        public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository, IProductRepository productRepository)
        {
            _productTransactionRepository = productTransactionRepository;
            _productRepository = productRepository;
        }
        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string completedBy)
        {
            //add to transaction record
            await _productTransactionRepository.ProduceAsync(productionNumber, product, quantity, completedBy);

            //decrease the quantity inventories

            //update the quantity of product
            product.Quantity += quantity;
            await _productRepository.UpdateProductAsync(product);
        }
    }
}
