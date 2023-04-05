using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.Interfaces
{
    public interface IViewInventoriesByNameUseCases
    {
        Task<IEnumerable<Inventory>> ExecuteAsync(string name = "");
    }
}