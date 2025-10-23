using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.DeviceDTOs
{
    public class DeviceForLeavingReviewDTO
    {
        public DeviceForLeavingReviewDTO(int id, string name, string brand, string model, string description, string color, int year)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Description = description;
            Color = color ?? throw new ArgumentNullException(nameof(color));
            Year = year;

        }

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the name of the device")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]

        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the Brand of the device")]
        public string Brand { get; set; }
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required]
        public string Model { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2024.")]
        public int Year { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DeviceForLeavingReviewDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Description == dTO.Description &&
                   Brand == dTO.Brand &&
                   Color == dTO.Color &&
                   Model == dTO.Model &&
                   Year == dTO.Year;
        }
    }
}
