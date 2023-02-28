using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interface
{
    public interface IViewProductsByIdUseCase
    {
        Task<Product?> ExecuteAsync(int prodId);
    }
}