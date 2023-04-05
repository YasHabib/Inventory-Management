using IMS.CoreBusiness;

namespace IMS.UseCases.Actvities.Interfaces
{
    public interface IProduceProductUseCase
    {
        Task ExecuteAsync(string productionNumber, Product product, int quantity, string completedBy);
    }
}