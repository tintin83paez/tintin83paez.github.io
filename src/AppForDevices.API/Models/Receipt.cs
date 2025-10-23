
namespace AppForDevices.API.Models
{
     
    public class Receipt
    {
        public Receipt()
        {
        }
        //public Receipt(string customerNameSurname, string deliveryAddress, PaymentMethodTypes paymentMethod, IList<ReceiptItem> receiptItems, DateTime receiptDate)
        //{
        //    TotalPrice = receiptItems.Sum(ri => ri.Repair.Cost);// "ri" stands for ReceiptItem //notice this pls
        //    CustomerNameSurname = customerNameSurname;
        //    DeliveryAddress = deliveryAddress;
        //    PaymentMethod = paymentMethod;
        //    ReceiptItems = receiptItems;
        //    ReceiptDate = receiptDate;
        //}

        //public Receipt(string customerNameSurname, string deliveryAddress, PaymentMethodTypes paymentMethod, IList<ReceiptItem> receiptItems, ApplicationUser applicationUser, DateTime receiptDate)
        //{
        //    TotalPrice = receiptItems.Sum(ri => ri.Repair.Cost);// "ri" stands for ReceiptItem //notice this pls
        //    CustomerNameSurname = customerNameSurname;
        //    DeliveryAddress = deliveryAddress;
        //    PaymentMethod = paymentMethod;
        //    ReceiptItems = receiptItems;
        //    ApplicationUser = applicationUser;
        //    ReceiptDate = receiptDate;
        //}

        public Receipt(
            //double totalPrice,
            string customerNameSurname,
            string deliveryAddress,
            PaymentMethodTypes paymentMethod,
            IList<ReceiptItem> receiptItems,
            ApplicationUser applicationUser,
            DateTime receiptDate
            )
        {
            TotalPrice = receiptItems.Sum(ri => ri.Repair.Cost); 
            CustomerNameSurname = customerNameSurname;
            DeliveryAddress = deliveryAddress;
            PaymentMethod = paymentMethod;
            ReceiptItems = receiptItems;
            ApplicationUser = applicationUser;
            ReceiptDate = receiptDate;
        }

        public int Id { get; set; }
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }
        [Display(Name = "Name and Surname")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(40,ErrorMessage ="Name and Surname contains more than 40 characters or less than 5",MinimumLength =5)]
        [RegularExpression(@"^[A-Za-z]+ [A-Za-z]+$", ErrorMessage = "El nombre completo debe contener al menos un nombre y un apellido separados por un espacio.")]
        public string CustomerNameSurname { get; set; }
        [StringLength(50,ErrorMessage ="Delivery Address contains more than 50 characters or less than 5",MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }
        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod {  get; set; }  

        public IList<ReceiptItem> ReceiptItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime ReceiptDate { get; set; }
        

        

        public override bool Equals(object? obj)
        {
            return obj is Receipt receipt &&
                   Id == receipt.Id &&
                   TotalPrice == receipt.TotalPrice &&
                   CustomerNameSurname == receipt.CustomerNameSurname &&
                   DeliveryAddress == receipt.DeliveryAddress &&
                   PaymentMethod == receipt.PaymentMethod &&
                   ReceiptItems.SequenceEqual(receipt.ReceiptItems) &&
                   ReceiptDate == receipt.ReceiptDate;
        }
    }
}
