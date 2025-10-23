using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.RentalDTOs
{
    public class RentalDetailDTO : RentalForCreateDTO //Related to second get Method
    {
        public RentalDetailDTO(int id, DateTime rentalDate, string customerUserName, string customerNameSurname, string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalItemDTO> rentalItems) 
        :base(customerNameSurname, customerNameSurname, deliveryAddress, paymentMethod, rentalDateFrom, rentalDateTo, rentalItems)
                {
                Id = id;
                RentalDate = rentalDate;             
        }
            public int Id { get; set; }
            public DateTime RentalDate { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is RentalDetailDTO dTO &&
                   base.Equals(obj) &&
                   TotalPrice == dTO.TotalPrice &&
                   Id == dTO.Id &&
                   CompareDate(RentalDate, dTO.RentalDate);
        }
    }
}

