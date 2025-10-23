
namespace AppForDevices.API.Models
{
    public class ReviewItem
    {
        public ReviewItem()
        {
        }

        public ReviewItem(int reviewId, Review review, int deviceId, Device device, string comments, int rating)
        {
            ReviewId = reviewId;
            Review = review ?? throw new ArgumentNullException(nameof(review));
            DeviceId = deviceId;
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Comments = comments ?? throw new ArgumentNullException(nameof(comments));
            Rating = rating;
        }

        public ReviewItem(int id, int reviewId, Review review, int deviceId, Device device, string comments, int rating)
        {
            Id = id;
            ReviewId = reviewId;
            Review = review ?? throw new ArgumentNullException(nameof(review));
            DeviceId = deviceId;
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Comments = comments ?? throw new ArgumentNullException(nameof(comments));
            Rating = rating;
        }

        [Key]
        public int Id { get; set; }

        // Relación con Review
        [Required]
        public int ReviewId { get; set; }
        public Review Review { get; set; }

        // Relación con Device
        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        
        [Required(ErrorMessage = "Comments are required.")]
        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters.")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItem item &&
                   Id == item.Id &&
                   ReviewId == item.ReviewId &&
                   DeviceId == item.DeviceId &&
                   EqualityComparer<Device>.Default.Equals(Device, item.Device) &&
                   Comments == item.Comments &&
                   Rating == item.Rating;
        }
    }
}
