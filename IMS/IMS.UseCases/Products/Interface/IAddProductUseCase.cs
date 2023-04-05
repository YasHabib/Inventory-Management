using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interface
{
    public interface IAddProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}