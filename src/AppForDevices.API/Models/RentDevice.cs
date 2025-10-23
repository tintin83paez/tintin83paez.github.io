

namespace AppForDevices.API.Models
{
    public class RentDevice
    {
        public RentDevice()
        {
        }
        public RentDevice(Device device, int quantity, Rental rental)
        {
            Device = device;
            Quantity = quantity;
            Rental = rental;
        }
        public RentDevice(int deviceId, Rental rental, double price)
        {
            DeviceId = deviceId;
            Rental = rental;
            Price = price;

        }

        public RentDevice(Rental rental, Device device)
        {
            Rental = rental;
            Device = device;
        }

        public RentDevice(int quantity, double price, Rental rental, Device device)
        {
            Quantity = quantity;
            Price = price;
            Rental = rental ?? throw new ArgumentNullException(nameof(rental));
            Device = device ?? throw new ArgumentNullException(nameof(device));
        }

        public RentDevice(int deviceId, int quantity, double price, int rentalId, Rental rental, Device device)
        {
            DeviceId = deviceId;
            Quantity = quantity;
            Price = price;
            RentalId = rentalId;
            Rental = rental ?? throw new ArgumentNullException(nameof(rental));
            Device = device ?? throw new ArgumentNullException(nameof(device));
        }

        public int DeviceId {  get; set; }
        public int Quantity {  get; set; }
        public double Price { get; set; }
        public int RentalId { get; set; }
        public Rental Rental { get; set; }
        public Device Device { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RentDevice device &&
                   DeviceId == device.DeviceId &&
                   Quantity == device.Quantity &&
                   Price == device.Price &&
                   RentalId == device.RentalId &&
                   EqualityComparer<Rental>.Default.Equals(Rental, device.Rental) &&
                   EqualityComparer<Device>.Default.Equals(Device, device.Device);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceId, RentalId);
        }
    }
}
