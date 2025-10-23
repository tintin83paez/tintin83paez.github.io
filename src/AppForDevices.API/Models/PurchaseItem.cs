

namespace AppForDevices.API.Models
{
    public class PurchaseItem
    {
        public PurchaseItem()
        {
        }

        public PurchaseItem(Device device, int quantity, Purchase purchase)
        {
            Device = device;
            Quantity = quantity;
            Purchase = purchase;
        }

        public PurchaseItem(Device device, int quantity, double price, string? description, Purchase purchase)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Quantity = quantity;
            Price = price;
            Description = description;
            Purchase = purchase ?? throw new ArgumentNullException(nameof(purchase));
        }

        public PurchaseItem(int deviceId, Device device, int quantity, double price, int purchaseId, string? description, Purchase purchase)
        {
            DeviceId = deviceId;
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Quantity = quantity;
            Price = price;
            PurchaseId = purchaseId;
            Description = description;
            Purchase = purchase ?? throw new ArgumentNullException(nameof(purchase));
        }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must provide a quantity higher than 1")]
        public int Quantity { get; set; }
        public double Price {  get; set; }
        public int PurchaseId {  get; set; }
        [StringLength(100, ErrorMessage = "The description can't be longer than 100 characters.")]
        public string? Description { get; set; }
        public Purchase Purchase { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItem item &&
                   DeviceId == item.DeviceId &&
                   EqualityComparer<Device>.Default.Equals(Device, item.Device) &&
                   Quantity == item.Quantity &&
                   Price == item.Price &&
                   PurchaseId == item.PurchaseId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceId, PurchaseId);
        }
    }
}
