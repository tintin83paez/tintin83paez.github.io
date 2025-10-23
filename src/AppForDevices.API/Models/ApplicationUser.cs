using Microsoft.AspNetCore.Identity;

namespace AppForDevices.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(String id, string userName, string name, string surname)
        {
            Id = id;
            UserName = userName;
            Email = userName;
            Name = name;
            Surname = surname;
        }
        public ApplicationUser(string name, string surname)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
        }

        public ApplicationUser()
        {
        }

        //Information of the user regarding using the application 
        //name, surname...
        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the name")]
        public string Name { get; set; }
        [Display(Name = "Surname")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please introduce the surname")]
        public string Surname { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ApplicationUser user &&
                   UserName == user.UserName &&
                   Id == user.Id &&
                   Name == user.Name &&
                   Surname == user.Surname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname);
        }
    }
}