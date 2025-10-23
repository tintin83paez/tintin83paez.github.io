using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.ReviewDTOs
{
    public class ReviewForCreateDTO
    {

        [StringLength(100, ErrorMessage = "Review title cannot exceed 100 characters.")]
        public string ReviewTitle { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public DateTime DateOfReview { get; set; } = DateTime.UtcNow;

        [StringLength(100, ErrorMessage = "Customer country cannot exceed 100 characters")]
        public string CustomerCountry { get; set; }


        public List<ReviewItemDTO> ReviewItems { get; set; }
    }
}
