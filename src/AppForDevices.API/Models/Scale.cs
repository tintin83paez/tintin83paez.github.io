
namespace AppForDevices.API.Models
{
    public class Scale
    {
        public Scale()
        {
        }
        public Scale(string name)
        {
            Name = name;
        }
        public Scale(string name, List<Repair> repairs)
        {
            Name = name;
            Repairs = repairs;
        }

        public Scale(int id, string name, List<Repair> repairs)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Repairs = repairs ?? throw new ArgumentNullException(nameof(repairs));
        }

        [Range(1,3,ErrorMessage ="Minimum 1, Maximum 3")]
        public int Id { get; set; }
        [StringLength(10,ErrorMessage ="ScaleName up to 10 letters",MinimumLength  = 1)]
        public string Name { get; set; }
        public List<Repair> Repairs { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Scale scale &&
                   Id == scale.Id &&
                   Name == scale.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
