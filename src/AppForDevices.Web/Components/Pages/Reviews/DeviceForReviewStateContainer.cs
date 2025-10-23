using AppForDevices.Web.API;

namespace AppForDevices.Web.Components.Pages.Reviews
{
    public class DeviceForReviewStateContainer
    {
        public ReviewForCreateDTO Review { get; set; }

        public DeviceForReviewStateContainer()
        {
            Reset();
        }

        // Reset the review state
        public void Reset()
        {
            Review = new ReviewForCreateDTO
            {
                ReviewItems = new List<ReviewItemDTO>()
            };
        }

        // Add a device to the review's list 
        public void AddDeviceToReview(DeviceForLeavingReviewDTO device)
        {
            // Only add if it’s not already in the review
            if (!Review.ReviewItems.Any(r => r.DeviceId == device.Id))
            {
                Review.ReviewItems.Add(new ReviewItemDTO
                {
                    DeviceId = device.Id,
                    Comments = "", 
                    Rating = 0     
                });
            }
        }

        // Remove a device 
        public void RemoveReviewItemFromReview(ReviewItemDTO reviewItem)
        {
            var itemToRemove = Review.ReviewItems.FirstOrDefault(r => r.DeviceId == reviewItem.DeviceId);
            if (itemToRemove != null)
            {
                Review.ReviewItems.Remove(itemToRemove);
            }
        }
    }
}
