using IMS.UseCases.Products.Interface;
using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Products
{
    public class ViewProductsByNameUseCases : IViewProductsByNameUseCases
    {
        private readonly IProductRepository _productRepository;
        public ViewProductsByNameUseCases(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> ExecuteAsyc(string name = "")
        {
            return await _productRepository.GetProductsByNameAsync(name);
        }
    }
}
