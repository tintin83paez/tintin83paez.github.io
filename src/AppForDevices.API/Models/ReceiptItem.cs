

namespace AppForDevices.API.Models
{
    [PrimaryKey(nameof(ReceiptId),nameof(RepairId))]
    public class ReceiptItem
    {
        public ReceiptItem()
        {
        }

        //public ReceiptItem(int repairId, Receipt receipt, string model)
        //{
        //    RepairId = repairId;
        //    Receipt = receipt;
        //    Model = model;
        //}

        public ReceiptItem(Repair repair, Receipt receipt, string model)
        {
            Repair = repair ?? throw new ArgumentNullException(nameof(repair));
            Receipt = receipt ?? throw new ArgumentNullException(nameof(receipt));
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        //public ReceiptItem(Repair repair, int repairId, Receipt receipt, int receiptId, string model)
        //{
        //    Repair = repair ?? throw new ArgumentNullException(nameof(repair));
        //    RepairId = repairId;
        //    Receipt = receipt ?? throw new ArgumentNullException(nameof(receipt));
        //    ReceiptId = receiptId;
        //    Model = model ?? throw new ArgumentNullException(nameof(model));
        //}

        public Repair Repair { get; set; }
        [Required]
        public int RepairId { get; set; }
        public Receipt Receipt { get; set; }
        [Required]
        public int ReceiptId { get; set; }
        [Required(ErrorMessage ="Model is required")]
        [StringLength(100, ErrorMessage = "The model name cannot exceed 100 characters.")]
        public string Model { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReceiptItem item &&
                   EqualityComparer<Repair>.Default.Equals(Repair, item.Repair) &&
                   RepairId == item.RepairId &&
                   ReceiptId == item.ReceiptId &&
                   Model == item.Model;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RepairId, ReceiptId);
        }
    }
}
