using System.ComponentModel.DataAnnotations;

namespace AppForDevices.Shared.RepairDTOs
{
    public class ReceiptItemDTO
    {
        public ReceiptItemDTO(
            int repairId,
            string name,
            double cost,
            string scale,
            string? description,
            string model
            )
        {
            RepairId = repairId;
            Name = name;
            Cost = cost;
            Scale = scale;
            Description = description;
            Model = model;
        }

        [Required]
        public int RepairId { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Scale { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Model is required")]
        [StringLength(100, ErrorMessage = "The model name cannot exceed 100 characters.")]
        public string Model { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReceiptItemDTO dTO &&
                   RepairId == dTO.RepairId &&
                   Name == dTO.Name &&
                   Cost == dTO.Cost &&
                   Scale == dTO.Scale &&
                   Description == dTO.Description &&
                   Model == dTO.Model;
        }
    }
}
