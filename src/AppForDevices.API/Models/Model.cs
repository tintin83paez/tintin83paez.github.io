
namespace AppForDevices.API.Models
{
    public class Model
    {
        public Model()
        {
        }
        public Model(string nameModel)
        {
            NameModel = nameModel;
        }

        public Model(string nameModel, List<Device> devices)
        {
            NameModel = nameModel;
            Devices = devices;
        }

        public Model(int id, string nameModel, List<Device> devices)
        {
            Id = id;
            NameModel = nameModel ?? throw new ArgumentNullException(nameof(nameModel));
            Devices = devices ?? throw new ArgumentNullException(nameof(devices));
        }

        [Key]
        public int Id { get; set; }
        [Display (Name = "Type of device")]
        [Required (AllowEmptyStrings =false, ErrorMessage = "The type is compulsory")]
        [StringLength (20, ErrorMessage = "Name of the Model can be neither longer than 20 characters nor shorter than 1", MinimumLength = 1)]
        public string NameModel { get; set; }
        public List<Device> Devices { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Model model &&
                   Id == model.Id &&
                   NameModel == model.NameModel;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NameModel);
        }
    }
}
