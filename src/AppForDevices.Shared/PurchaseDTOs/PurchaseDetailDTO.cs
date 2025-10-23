using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.PurchaseDTOs
{
    public class PurchaseDetailDTO : PurchaseForCreateDTO
    {
        public PurchaseDetailDTO()
        {
        }

      

        public PurchaseDetailDTO(int id, string deliveryAddress, IList<PurchaseItemDTO> purchaseItems, string userName, string nameSurname, PaymentMethodTypes paymentMethod, DateTime purchaseDate, double totalPrice, int totalQuantity) :
            base(deliveryAddress, purchaseItems, userName, nameSurname, paymentMethod)
        {
            Id = id;
            this.purchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
        }
        
        public PurchaseDetailDTO(string deliveryAddress, IList<PurchaseItemDTO> purchaseItems, string userName, string nameSurname, PaymentMethodTypes paymentMethod, DateTime purchaseDate, double totalPrice, int totalQuantity) :
            base(deliveryAddress, purchaseItems, userName, nameSurname, paymentMethod)
        {
       
            this.purchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
        }
        public PurchaseDetailDTO(int id, string deliveryAddress, IList<PurchaseItemDTO> purchaseItems, string userName, string nameSurname, PaymentMethodTypes paymentMethod, DateTime purchaseDate) :
            base(deliveryAddress, purchaseItems, userName, nameSurname, paymentMethod)
        {
            this.Id = id;
            this.purchaseDate = purchaseDate;
        }
        public PurchaseDetailDTO(string deliveryAddress, IList<PurchaseItemDTO> purchaseItems, string userName, string nameSurname, PaymentMethodTypes paymentMethod, DateTime purchaseDate) :
            base(deliveryAddress, purchaseItems, userName, nameSurname, paymentMethod)
        {
            this.purchaseDate = purchaseDate;
        }

        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime purchaseDate { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Id == dTO.Id &&
                   (purchaseDate.Subtract(dTO.purchaseDate) < new TimeSpan(0, 1, 0));
        }
    }
}
