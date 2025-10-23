using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForDevices.Shared.RentalDTOs
{
    public class RentalForCreateDTO //Related to post Method
    {
        public RentalForCreateDTO(string name, string surname, string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalItemDTO> rentalItems)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            RentalItems = rentalItems ?? throw new ArgumentNullException(nameof(rentalItems));
        }
        public RentalForCreateDTO()
        {
            RentalItems = new List<RentalItemDTO>();
        }
        public DateTime RentalDateFrom { get; set; }
        public DateTime RentalDateTo { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [EmailAddress]
        [Required]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string Surname { get; set; }

        public IList<RentalItemDTO> RentalItems { get; set; }
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        private int NumberOfDays
        {
            get
            {
                return (RentalDateTo - RentalDateFrom).Days;
            }
        }
        [Display(Name = "Total Price")]
        [JsonPropertyName("TotalPrice")]
        public double TotalPrice
        {
            get
            {
                return RentalItems.Sum(ri => ri.PriceForRental * NumberOfDays);
            }
        }
        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }
        public override bool Equals(object? obj)
        {
            return obj is RentalForCreateDTO dTO &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   PaymentMethod == dTO.PaymentMethod &&
                   CompareDate(RentalDateFrom, dTO.RentalDateFrom) &&
                   CompareDate(RentalDateTo, dTO.RentalDateTo) &&
                   TotalPrice == dTO.TotalPrice &&
                   RentalItems.SequenceEqual(dTO.RentalItems);
        }

    }
}
