using IMS.CoreBusiness;
using IMS.WebApp.ViewModelsValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class SellViewModel
    {
        [Required]
        public string SalesOrderNumber { get; set; } = string.Empty;
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range (minimum:1, maximum: int.MaxValue, ErrorMessage ="Quantity has to be greatar than 1")]
        public int QuantityToSell { get; set; }
        [Required]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "Price is required")]
        [Sell_EnsureEnoughProductQuantity]
        public double UnitPrice { get; set; }
        public Product? Products { get; set; }
    }
}
