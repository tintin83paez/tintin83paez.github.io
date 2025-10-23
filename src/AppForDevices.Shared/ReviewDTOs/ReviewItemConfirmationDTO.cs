using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.Shared.ReviewDTOs
{
    public class ReviewItemConfirmationDTO
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
    }
}
