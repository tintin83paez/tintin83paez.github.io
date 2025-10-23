using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.PurchaseDTOs
{
    public class PurchaseItemDTO
    {
        public PurchaseItemDTO()
        {
        }

        public PurchaseItemDTO(int deviceId, string name, double priceForPurchase, string model, int quantity)
        {
            DeviceId = deviceId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PriceForPurchase = priceForPurchase;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Quantity = quantity;
        }

        public PurchaseItemDTO(int deviceId, string name, double priceForPurchase, string model, int quantity, string brand, string color) : this(deviceId, name, priceForPurchase, model, quantity)
        {
            Brand = brand;
            Color = color;
        }

        public int DeviceId { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [Display(Name = "Price For Purchase")]
        public double PriceForPurchase { get; set; }

        public string Model { get; set; }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "You must provide a valid quantity")]
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItemDTO dTO &&
                   DeviceId == dTO.DeviceId &&
                   Name == dTO.Name &&
                   PriceForPurchase == dTO.PriceForPurchase &&
                   Model == dTO.Model &&
                   Quantity == dTO.Quantity;
        }
    }
}
