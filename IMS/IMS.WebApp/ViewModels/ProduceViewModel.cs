using IMS.CoreBusiness;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using IMS.WebApp.ViewModelsValidations;

namespace IMS.WebApp.ViewModels
{
    public class ProduceViewModel
    {
        [Required]
        public string ProductionNumber {get;set;} = string.Empty;

        [Required]
        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage ="Please select a product")]
        public int ProductId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please enter quantity")]
        [Produce_EnsureEnoughInventoryQuantity]
        public int QuantityProduced { get; set; }

        public Product? Products { get; set; }
    }
}
