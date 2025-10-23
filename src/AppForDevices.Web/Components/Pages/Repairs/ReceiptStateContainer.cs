using AppForDevices.Web.API;
namespace AppForDevices.Web.Components.Pages.Repairs
{
    public class ReceiptStateContainer
    {
        //we use
        public ReceiptForCreateDTO Receipt { get; private set; } = new ReceiptForCreateDTO()
        {
            ReceiptItems = new List<ReceiptItemDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddRepairToReceipt(RepairDTO repair)
        {
            if (!Receipt.ReceiptItems.Any(ri => ri.RepairId == repair.Id))
                Receipt.ReceiptItems.Add(new ReceiptItemDTO()
                {
                    RepairId = repair.Id,
                    Name = repair.Name,
                    Cost = repair.Cost,
                    Scale = repair.Scale,
                    Description = repair.Description,
                    Model = ""
                }
            );
            ComputeTotalPrice();
        }

        private void ComputeTotalPrice()
        {
            Receipt.TotalPrice = Receipt.ReceiptItems.Sum(ri => ri.Cost);
        }

        public void RemoveReceiptItem(ReceiptItemDTO item)
        {
            Receipt.ReceiptItems.Remove(item);
            ComputeTotalPrice();
        }

        public void ClearRentingCart()
        {
            Receipt.ReceiptItems.Clear();
            Receipt.TotalPrice = 0;
        }

        public void ReceiptProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Receipt = new ReceiptForCreateDTO()
            {
                ReceiptItems = new List<ReceiptItemDTO>()
            };
        }
    }
}
