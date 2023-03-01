using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class Produce_EnsureEnoughInventoryQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {
            var produceViewModel = validationContext.ObjectInstance as ProduceViewModel;
            if( produceViewModel != null)
            {
                if(produceViewModel.Products != null && produceViewModel.Products.ProductInventories != null)
                {
                    foreach(var pi in produceViewModel.Products.ProductInventories)
                    {
                        if(pi.Inventory != null && (pi.Inventory.Quantity * produceViewModel.QuantityProduced) > pi.Inventory.Quantity)
                        {
                            return new ValidationResult($"The inventory {pi.Inventory.InventoryName} is not enough to produce {produceViewModel.QuantityProduced} products", new[] {validationContext.MemberName});
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
