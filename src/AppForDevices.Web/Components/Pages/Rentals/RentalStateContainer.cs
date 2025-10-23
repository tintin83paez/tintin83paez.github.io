using AppForDevices.Web.API;
namespace AppForDevices.Web.Components.Pages.Rentals
{
    public class RentalStateContainer
    {
        public RentalForCreateDTO Rental { get; set; } = new RentalForCreateDTO();
        public RentalStateContainer()
        {
            Rental = new RentalForCreateDTO();
            Rental.RentalItems = new List<RentalItemDTO>();
        }

        public void Reset()
        {
            Rental = new RentalForCreateDTO();
            Rental.RentalItems = new List<RentalItemDTO>();
        }

        public void AddDeviceToRental(DeviceForRentalDTO device)
        {
            if (!Rental.RentalItems.Any(pi => pi.DeviceId == device.Id))
                Rental.RentalItems.Add(new RentalItemDTO()
                {
                    Name = device.Name,
                    PriceForRental = device.RentalPrice,
                    DeviceId = device.Id,
                    Model = device.Model


                });
        }

        public void RemoveRentalItemFromRental(RentalItemDTO rent)
        {
            Rental.RentalItems.Remove(Rental.RentalItems.FirstOrDefault(pi => pi.DeviceId == rent.DeviceId));
        }
    }
}