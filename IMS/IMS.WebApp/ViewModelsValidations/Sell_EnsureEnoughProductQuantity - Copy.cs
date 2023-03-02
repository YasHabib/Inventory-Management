using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {
            var sellViewModel = validationContext.ObjectInstance as SellViewModel;
            if( sellViewModel != null)
            {
                if(sellViewModel.Products != null)
                {
                    
                        if(sellViewModel.Products.Quantity < sellViewModel.QuantityToSell)
                        {
                            return new ValidationResult($"There isn't enough product. There are only {sellViewModel.Products.Quantity} products.", new[] {validationContext.MemberName});
                        }
                    
                }
            }
            return ValidationResult.Success;
        }
    }
}
