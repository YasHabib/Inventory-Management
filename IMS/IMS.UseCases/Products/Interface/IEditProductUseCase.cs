using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interface
{
    public interface IEditProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}