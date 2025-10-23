
namespace AppForDevices.API.Models
{
    public class Repair
    {
        public Repair()
        {
        }

        public Repair (string name, string? description, float cost, Scale scale)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Cost = cost;
            Scale = scale ?? throw new ArgumentNullException(nameof(scale));
        }
        public Repair(string name, string? description, float cost, Scale scale, int scaleId, IList<ReceiptItem> receiptItems)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Cost = cost;
            Scale = scale ?? throw new ArgumentNullException(nameof(scale));
            ScaleId = scaleId;
            ReceiptItems = receiptItems ?? throw new ArgumentNullException(nameof(receiptItems));
        }

        public Repair(int id, string name, string? description, float cost, Scale scale, int scaleId, IList<ReceiptItem> receiptItems)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Cost = cost;
            Scale = scale ?? throw new ArgumentNullException(nameof(scale));
            ScaleId = scaleId;
            ReceiptItems = receiptItems ?? throw new ArgumentNullException(nameof(receiptItems));
        }

        public int Id { get; set; }
        [StringLength(50,ErrorMessage ="Name of the Repair is more than 50 characters or less than 2",MinimumLength =2)]
        public string Name {  get; set; }
        public string ? Description {  get; set; } // Description can be nullable
        public float Cost {  get; set; }   
        public Scale Scale { get; set; }
        [Required]
        public int ScaleId { get;set; }
        public IList<ReceiptItem> ReceiptItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Repair repair &&
                   Id == repair.Id &&
                   Name == repair.Name &&
                   Description == repair.Description &&
                   Cost == repair.Cost &&
                   EqualityComparer<Scale>.Default.Equals(Scale, repair.Scale) &&
                   ScaleId == repair.ScaleId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
