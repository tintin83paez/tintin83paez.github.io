namespace AppForDevices.Shared.RepairDTOs
{
    public class ReceiptDetailDTO : ReceiptForCreateDTO
    {
        public ReceiptDetailDTO(
            int id,
            DateTime receiptDate,
            string customerUserName,
            PaymentMethodTypes paymentMethod,
            string deliveryAddress,
            string customerNameSurname,
            IList<ReceiptItemDTO> receiptItems
            ) : base(
                
                deliveryAddress,
                customerNameSurname,
                customerUserName,
                receiptItems,
                paymentMethod
                )
        {
            Id = id;
            ReceiptDate = receiptDate;
        }

        public int Id { get; set; }
        /// <summary>
        /// This is ReceiptDate
        /// </summary>
        public DateTime ReceiptDate { get; set; }
        /// <summary>
        /// Equals Method
        /// </summary>
        /// <param name="obj"> 
        /// Object to compare 
        /// </param>
        /// <returns> 
        /// It returns wheter this object and obj parameter are equal (boolean)
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is ReceiptDetailDTO dTO &&
                   base.Equals(obj) &&
                   TotalPrice == dTO.TotalPrice &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   CustomerNameSurname == dTO.CustomerNameSurname &&
                   PaymentMethod == dTO.PaymentMethod &&
                   Id == dTO.Id &&
                   ReceiptDate == dTO.ReceiptDate;
        }
    }

}
