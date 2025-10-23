

namespace AppForDevices.API.Models
{
    public class Rental
    {
        public Rental()
        {
        }

        public Rental(string name, string surname, string deliveryAddress, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentDevice> rentDevices, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            Name = name; // customerUserName
            Surname = surname; // customerNameSurname (surname)
            DeliveryAddress = deliveryAddress;
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            RentDevices = rentDevices; // rentalItems
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser;
        }

        public Rental(string name, string surname, string deliveryAddress, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double totalPrice, IList<RentDevice> rentDevices, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            TotalPrice = totalPrice;
            RentDevices = rentDevices ?? throw new ArgumentNullException(nameof(rentDevices));
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
        }

        public Rental(string name, string surname, string deliveryAddress, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double totalPrice, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            TotalPrice = totalPrice;
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
        }

        public Rental(int id, string name, string surname, string deliveryAddress, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double totalPrice, IList<RentDevice> rentDevices, PaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            TotalPrice = totalPrice;
            RentDevices = rentDevices ?? throw new ArgumentNullException(nameof(rentDevices));
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
        }

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce your name")]
        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce your surname")]
        [StringLength(30, ErrorMessage = "Surame cannot be longer than 30 characters.")]
        public string Surname { get; set; }
        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce an address")]
        public string DeliveryAddress { get; set; }
        public DateTime RentalDate { get; set; }
        [Display(Name = "Rent initial date")]
        [Required]
        public DateTime RentalDateFrom { get; set; }
        [Display(Name = "Rent final date")]
        [Required]
        public DateTime RentalDateTo { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total Price")]
        public double TotalPrice    { get; set; }
        public IList<RentDevice> RentDevices { get; set; } = new List<RentDevice>();
        [Display(Name = "Payment Method")]
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Rental rental &&
                   Id == rental.Id &&
                   Name == rental.Name &&
                   Surname == rental.Surname &&
                   DeliveryAddress == rental.DeliveryAddress &&
                   RentalDate == rental.RentalDate &&
                   RentalDateFrom == rental.RentalDateFrom &&
                   RentalDateTo == rental.RentalDateTo &&
                   TotalPrice == rental.TotalPrice &&
                   EqualityComparer<IList<RentDevice>>.Default.Equals(RentDevices, rental.RentDevices) &&
                   PaymentMethod == rental.PaymentMethod &&
                   EqualityComparer<ApplicationUser>.Default.Equals(ApplicationUser, rental.ApplicationUser);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, RentalDate);
        }
    }
}
