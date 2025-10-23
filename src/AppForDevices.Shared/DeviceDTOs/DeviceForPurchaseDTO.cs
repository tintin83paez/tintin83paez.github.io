using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.DeviceDTOs
{
    public class DeviceForPurchaseDTO
    {
        public DeviceForPurchaseDTO(int id, string name, string brand, string color, double priceforpurchase, string model)
        {
            Id = id;
            Name = name;
            Model = model;
            Color = color;
            Brand = brand;
            this.priceForPurchase = priceforpurchase;
        }

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the name of the device")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public String Model { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public double priceForPurchase { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DeviceForPurchaseDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Model == dTO.Model &&
                   Color == dTO.Color &&
                   Brand == dTO.Brand &&
                   priceForPurchase == dTO.priceForPurchase;
        }
    }
}
