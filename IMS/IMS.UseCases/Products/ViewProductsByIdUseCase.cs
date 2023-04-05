using IMS.CoreBusiness;
using IMS.UseCases.Products.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Products
{
    public class ViewProductsByIdUseCase : IViewProductsByIdUseCase
    {
        private IProductRepository _productRepository;
        public ViewProductsByIdUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> ExecuteAsync(int prodId)
        {
            return await _productRepository.GetProductByIdAsync(prodId);
        }
    }
}
