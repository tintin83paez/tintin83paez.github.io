


namespace AppForDevices.API.Models
{
    public class Purchase
    {
        public Purchase()
        {
        }

        public Purchase(string deliveryAddress, string customerUserName, string customerUserSurname, DateTime purchaseDate, IList<PurchaseItem> purchaseItems, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            DeliveryAddress = deliveryAddress;
            CustomerUserName = customerUserName;
            CustomerUserSurname = customerUserSurname;
            PurchaseDate = purchaseDate;
            PurchaseItems = purchaseItems;
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser;
           
        }

        public Purchase(string deliveryAddress, string customerUserName, string customerUserSurname, DateTime purchaseDate, double totalPrice, int totalQuantity, IList<PurchaseItem> purchaseItems, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            CustomerUserName = customerUserName ?? throw new ArgumentNullException(nameof(customerUserName));
            CustomerUserSurname = customerUserSurname ?? throw new ArgumentNullException(nameof(customerUserSurname));
            PurchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser;
        }

        public Purchase(int id, string deliveryAddress, string customerUserName, string customerUserSurname, DateTime purchaseDate, double totalPrice, int totalQuantity, IList<PurchaseItem> purchaseItems, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            Id = id;
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            CustomerUserName = customerUserName ?? throw new ArgumentNullException(nameof(customerUserName));
            CustomerUserSurname = customerUserSurname ?? throw new ArgumentNullException(nameof(customerUserSurname));
            PurchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
        }

        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the address for delivery.")]
        public string DeliveryAddress { get; set; }  
        [Display(Name = "Name of the customer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the name")]
        public string CustomerUserName { get; set; }
        [Display(Name = "Surname of the customer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the surname")]
        public string CustomerUserSurname { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
        [Display(Name = "Payment Method")]
        [Required()]
        public PaymentMethodTypes PaymentMethod { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Purchase purchase &&
                   Id == purchase.Id &&
                   DeliveryAddress == purchase.DeliveryAddress &&
                   CustomerUserName == purchase.CustomerUserName &&
                   CustomerUserSurname == purchase.CustomerUserSurname &&
                   PurchaseDate == purchase.PurchaseDate &&
                   TotalPrice == purchase.TotalPrice &&
                   TotalQuantity == purchase.TotalQuantity &&
                   EqualityComparer<IList<PurchaseItem>>.Default.Equals(PurchaseItems, purchase.PurchaseItems) &&
                   PaymentMethod == purchase.PaymentMethod &&
                   EqualityComparer<ApplicationUser>.Default.Equals(ApplicationUser, purchase.ApplicationUser);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, PurchaseDate);
        }
    }
}
