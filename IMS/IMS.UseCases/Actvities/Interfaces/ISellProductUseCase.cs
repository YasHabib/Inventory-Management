using IMS.CoreBusiness;

namespace IMS.UseCases.Actvities.Interfaces
{
    public interface ISellProductUseCase
    {
        Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double UnitPrice, string completedBy);
    }
}