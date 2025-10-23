using AppForDevices.Web.API;
namespace AppForDevices.Web.Components.Pages.Purchases
{
    public class PurchaseStateContainer
    {
        public PurchaseForCreateDTO Purchase { get; set; } = new PurchaseForCreateDTO();
        public PurchaseStateContainer()
        {
            Purchase = new PurchaseForCreateDTO();
            Purchase.PurchaseItems = new List<PurchaseItemDTO>();
        }

        public void Reset()
        {
            Purchase = new PurchaseForCreateDTO();
            Purchase.PurchaseItems = new List<PurchaseItemDTO>();
        }

        public void AddDeviceToPurchase(DeviceForPurchaseDTO device)
        {
            if (!Purchase.PurchaseItems.Any(pi => pi.DeviceId == device.Id))
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    Name = device.Name,
                    PriceForPurchase = device.PriceForPurchase,
                    DeviceId = device.Id,
                    Model = device.Model,
                    Brand = device.Brand,
                    Color = device.Color

                });
        }

        public void RemovePurchaseItemFromPurchase(PurchaseItemDTO purchase)
        {
            Purchase.PurchaseItems.Remove(Purchase.PurchaseItems.FirstOrDefault(pi => pi.DeviceId == purchase.DeviceId));
        }
    }
}
