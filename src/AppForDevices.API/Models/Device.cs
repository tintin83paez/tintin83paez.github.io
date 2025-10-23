
namespace AppForDevices.API.Models
{
    public class Device
    {
        public Device()
        {
        }

        public Device(string name, string description, string brand, string color, int year, Model model)
        {
            Name = name;
            Description = description;
            Brand = brand;
            Color = color;
            Year = year;
            Model = model;
        }

        public Device(string name, string description, string brand, string color, double priceForPurchase, int quantityForPurchase, int year, Model model, double priceForRent, int quantityForRent, QualityType quality)
        {
            Name = name;
            Description = description;
            Brand = brand;
            Color = color;
            this.priceForPurchase = priceForPurchase;
            this.quantityForPurchase = quantityForPurchase;
            Year = year;
            Model = model;
            this.priceForRent = priceForRent;
            this.quantityForRent = quantityForRent;
            Quality = quality;
        }

        public Device(string name, string description, string brand, string color, double priceForPurchase, int quantityForPurchase, int year, Model model, IList<PurchaseItem> purchaseItems, double priceForRent, int quantityForRent, QualityType quality, IList<RentDevice> rentedDevices, IList<ReviewItem> reviewItems)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Color = color ?? throw new ArgumentNullException(nameof(color));
            this.priceForPurchase = priceForPurchase;
            this.quantityForPurchase = quantityForPurchase;
            Year = year;
            Quality = quality;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
            this.priceForRent = priceForRent;
            this.quantityForRent = quantityForRent;
            RentedDevices = rentedDevices ?? throw new ArgumentNullException(nameof(rentedDevices));
            ReviewItems = reviewItems ?? throw new ArgumentNullException(nameof(reviewItems));
        }

        public Device(int id, string name, string description, string brand, string color, double priceForPurchase, int quantityForPurchase, int year, Model model, IList<PurchaseItem> purchaseItems, double priceForRent, int quantityForRent, QualityType quality, IList<RentDevice> rentedDevices, IList<ReviewItem> reviewItems)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Color = color ?? throw new ArgumentNullException(nameof(color));
            this.priceForPurchase = priceForPurchase;
            this.quantityForPurchase = quantityForPurchase;
            Year = year;
            Quality = quality;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
            this.priceForRent = priceForRent;
            this.quantityForRent = quantityForRent;
            RentedDevices = rentedDevices ?? throw new ArgumentNullException(nameof(rentedDevices));
            ReviewItems = reviewItems ?? throw new ArgumentNullException(nameof(reviewItems));
        }
        

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the name of the device")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [Display(Name = "Brand")]
        [Required (AllowEmptyStrings = false, ErrorMessage = "Please introduce the Brand of the device")]

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public string Brand { get; set; }
        [Display(Name = "Color")]
        public String Color { get; set; }

        [DataType(DataType.Currency)]
        [Range(5, float.MaxValue, ErrorMessage = "Minimum price is 5 ")]
        [Display(Name = "Price For Purchase")]
        public double priceForPurchase { get; set; }

        [Display(Name = "Quantity For Purchase")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int quantityForPurchase { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2024.")]
        public int Year { get; set; }

        [Required]
        public Model Model { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
        [DataType(DataType.Currency)]
        [Range(5, float.MaxValue, ErrorMessage = "Minimum price is 5 ")]
        [Display(Name = "Price For Purchase")]
        public double priceForRent { get; set; }
        [Display(Name = "Quantity For Rent")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Renting is 1")]
        public int quantityForRent { get; set; }
        public QualityType Quality { get; set; }
        public IList<RentDevice> RentedDevices { get; set; }
        public IList<ReviewItem> ReviewItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Device device &&
                   Id == device.Id &&
                   Name == device.Name &&
                   Description == device.Description &&
                   Brand == device.Brand &&
                   Color == device.Color &&
                   priceForPurchase == device.priceForPurchase &&
                   quantityForPurchase == device.quantityForPurchase &&
                   Year == device.Year &&
                   EqualityComparer<Model>.Default.Equals(Model, device.Model) &&
                   priceForRent == device.priceForRent &&
                   quantityForRent == device.quantityForRent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
