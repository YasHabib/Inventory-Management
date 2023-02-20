using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interface
{
    public interface IViewProductsByNameUseCases
    {
        Task<IEnumerable<Product>> ExecuteAsyc(string name = "");
    }
}