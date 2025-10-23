using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.DeviceDTOs
{
    public class DeviceForRentalDTO
    {
        public DeviceForRentalDTO(int id, string name, string model, string brand, string color, int quantity, double rentalPrice, QualityType quality)
        {
            Id = id;
            Name = name;
            Model = model;
            Brand = brand;
            Color = color;
            Quantity = quantity;
            RentalPrice = rentalPrice;
            Quality = quality;
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the name of the device")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        public string Model { get; set; }


        public string Brand { get; set; }

        public string Color { get; set; }

        public int Quantity { get; set; }

        public double RentalPrice { get; set; }

        public QualityType Quality { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DeviceForRentalDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Model == dTO.Model &&
                   Brand == dTO.Brand &&
                   Color == dTO.Color &&
                   Quantity == dTO.Quantity &&
                   RentalPrice == dTO.RentalPrice &&
                   Quality == dTO.Quality;
        }
    }
}