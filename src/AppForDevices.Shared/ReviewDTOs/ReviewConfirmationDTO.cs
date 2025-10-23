using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.ReviewDTOs
{
    public class ReviewConfirmationDTO
    {
        public int ReviewId { get; set; } // Add this line
        public string ReviewTitle { get; set; }
        public DateTime DateOfReview { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerId { get; set; }
        public double OverallRating { get; set; }
        public List<ReviewItemConfirmationDTO> ReviewItems { get; set; }
    }
}
