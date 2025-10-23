using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForDevices.API.Models
{
    public class Review
    {
        public Review()
        {
        }

        public Review(string reviewTitle, DateTime dateOfReview, string customerCountry, string customerId, IList<ReviewItem> reviewItems)
        {
            ReviewTitle = reviewTitle;
            DateOfReview = dateOfReview;
            CustomerCountry = customerCountry;
            CustomerId = customerId;
            ReviewItems = reviewItems;
        }

        public Review(string reviewTitle, DateTime dateOfReview, string customerCountry, string customerId, ApplicationUser applicationUser, IList<ReviewItem> reviewItems)
        {
            ReviewTitle = reviewTitle;
            CustomerCountry = customerCountry;
            DateOfReview = dateOfReview;
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
            ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
            ReviewItems = reviewItems ?? throw new ArgumentNullException(nameof(reviewItems));
        }

        public Review(int reviewId, string reviewTitle, DateTime dateOfReview, string customerId, ApplicationUser applicationUser, IList<ReviewItem> reviewItems)
        {
            ReviewId = reviewId;
            ReviewTitle = reviewTitle ?? throw new ArgumentNullException(nameof(reviewTitle));
            DateOfReview = dateOfReview;
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
            ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
            ReviewItems = reviewItems ?? throw new ArgumentNullException(nameof(reviewItems));
        }

        [Key]
        public int ReviewId { get; set; }

        [StringLength(100, ErrorMessage = "Review title cannot exceed 100 characters.")]
        public string ReviewTitle { get; set; }

        [Required]
        public DateTime DateOfReview { get; set; } = DateTime.UtcNow;

        [StringLength(100, ErrorMessage = "Customer country cannot exceed 100 characters")]
        public string CustomerCountry { get; set; }

        [Required]
        public string CustomerId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }


        public IList<ReviewItem> ReviewItems { get; set; }

        [NotMapped]
        public double OverallRating
        {
            get
            {
                if (ReviewItems != null && ReviewItems.Count > 0)
                {
                    return ReviewItems.Average(item => item.Rating);
                }
                else
                {
                    return 0;
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Review review &&
                   ReviewId == review.ReviewId &&
                   ReviewTitle == review.ReviewTitle &&
                   DateOfReview == review.DateOfReview &&
                   CustomerCountry == review.CustomerCountry &&
                   CustomerId == review.CustomerId &&
                   EqualityComparer<ApplicationUser>.Default.Equals(ApplicationUser, review.ApplicationUser) &&
                   EqualityComparer<IList<ReviewItem>>.Default.Equals(ReviewItems, review.ReviewItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReviewId, ReviewTitle);
        }
    }
}
