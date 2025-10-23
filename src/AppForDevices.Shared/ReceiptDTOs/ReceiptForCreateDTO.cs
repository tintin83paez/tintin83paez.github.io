using System.ComponentModel.DataAnnotations;

namespace AppForDevices.Shared.RepairDTOs
{
    public class ReceiptForCreateDTO
    {
        //public ReceiptForCreateDTO(
        //    string deliveryAddress,
        //    string customerNameSurname,
        //    IList<ReceiptItemDTO> receiptItems
        //    )
        //{
        //    DeliveryAddress = deliveryAddress;
        //    CustomerNameSurname = customerNameSurname;
        //    ReceiptItems = receiptItems;
        //}
        public ReceiptForCreateDTO()
        {
            ReceiptItems = new List<ReceiptItemDTO>();
        }

        public ReceiptForCreateDTO(
            
            string deliveryAddress,
            string customerNameSurname,
            string customerUserName,
            IList<ReceiptItemDTO> receiptItems,
            PaymentMethodTypes paymentMethod
            )
        {
            
            DeliveryAddress = deliveryAddress;
            CustomerNameSurname = customerNameSurname;
            this.customerUserName = customerUserName;
            ReceiptItems = receiptItems;
            PaymentMethod = paymentMethod;
        }


        /// <summary>
        /// This is the totalPrice
        /// </summary>
        [Display(Name = "Total Price")]
        public double TotalPrice {
            get 
            {
                return ReceiptItems.Sum(ri => ri.Cost); 
            } 
        }
        /// <summary>
        /// This is just the DeliveryAddress
        /// </summary>
        [StringLength(50, ErrorMessage = "Delivery Address contains more than 50 characters or less than 5", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }
        /// <summary>
        ///  This is just the CustomerNameSurname
        /// </summary>
        [Display(Name = "Name and Surname")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(40, ErrorMessage = "Name and Surname contains more than 40 characters or less than 5", MinimumLength = 5)]
        [RegularExpression(@"^[A-Za-z]+ [A-Za-z]+$", ErrorMessage = "El nombre completo debe contener al menos un nombre y un apellido separados por un espacio.")]
        public string CustomerNameSurname { get; set; }
        /// <summary>
        /// ListOfReceiptItems <-- We must put it into a different DTO
        /// </summary>
        //public IList<ReceiptItem> ReceiptItems { get; set; }
        public string customerUserName { get;set; }
        public IList<ReceiptItemDTO> ReceiptItems { get; set; }
        /// <summary>
        /// Payment Method
        /// </summary>
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReceiptForCreateDTO dTO &&
                   TotalPrice == dTO.TotalPrice &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   CustomerNameSurname == dTO.CustomerNameSurname &&
                   customerUserName == dTO.customerUserName &&
                   ReceiptItems.SequenceEqual(dTO.ReceiptItems) && // si no pongo SequenceEqual me da error :[
                   PaymentMethod == dTO.PaymentMethod;
        }
    }
}
