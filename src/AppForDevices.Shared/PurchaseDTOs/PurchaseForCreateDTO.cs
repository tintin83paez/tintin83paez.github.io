using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace AppForDevices.Shared.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {
        public PurchaseForCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>();
        }

        public PurchaseForCreateDTO(string deliveryAddress, IList<PurchaseItemDTO> purchaseItems, string userName, string nameSurname, PaymentMethodTypes paymentMethod)
        {
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            NameSurname = nameSurname ?? throw new ArgumentNullException(nameof(nameSurname));
            PaymentMethod = paymentMethod;
            
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the address for delivery.")]
        public string DeliveryAddress { get; set; }
        [ValidateComplexType]
        public IList<PurchaseItemDTO> PurchaseItems { get; set; }
        [Display(Name = "Total Price")]
        public double TotalPrice
        {
            get
            {
                return PurchaseItems.Sum(pi => pi.Quantity * pi.PriceForPurchase);
            }
        }

        public int TotalQuantity {
            get
            {
                return PurchaseItems.Sum(pi=>pi.Quantity);

            }
        }

        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string NameSurname { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodTypes PaymentMethod {  get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems) &&
                   TotalPrice == dTO.TotalPrice &&
                   TotalQuantity == dTO.TotalQuantity &&
                   UserName == dTO.UserName &&
                   NameSurname == dTO.NameSurname &&
                   PaymentMethod == dTO.PaymentMethod;
        }
    }
}
