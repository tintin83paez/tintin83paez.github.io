using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.ReviewDTOs
{
    public class ReviewItemDTO
    {
        [Required]
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "Comments are required.")]
        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters.")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }
    }
}
