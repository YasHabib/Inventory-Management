using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class PurchaseViewModel
    {
        [Required]
        public string PurchaseOrderNumber {get;set;} = string.Empty;
        [Required]
        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage ="Please select a inventory")]
        public int InventoryId { get; set; }
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please enter quantity")]
        public int QuantityPurchased { get; set; }
        public double InventoryPrice { get; set; }
    }
}
