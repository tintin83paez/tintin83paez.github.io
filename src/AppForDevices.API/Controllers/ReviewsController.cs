using AppForDevices.Shared.ReviewDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using System.Reflection;

namespace AppForDevices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(ApplicationDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewConfirmationDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.ReviewItems)
                    .ThenInclude(ri => ri.Device) // Include Device in ReviewItems
                        .ThenInclude(d => d.Model) // Include Model in Device
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }

            // Map to ReviewConfirmationDTO
            var reviewDto = new ReviewConfirmationDTO
            {
                ReviewId = review.ReviewId,
                ReviewTitle = review.ReviewTitle,
                DateOfReview = review.DateOfReview,
                CustomerCountry = review.CustomerCountry,
                CustomerId = review.CustomerId,
                OverallRating = review.ReviewItems.Average(ri => ri.Rating),
                ReviewItems = review.ReviewItems.Select(ri => new ReviewItemConfirmationDTO
                {
                    DeviceName = ri.Device.Name,
                    Rating = ri.Rating,
                    Comments = ri.Comments,
                    Color = ri.Device.Color,
                    Model = ri.Device.Model.NameModel
                }).ToList()
            };

            return Ok(reviewDto);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewConfirmationDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateReview([FromBody] ReviewForCreateDTO reviewForCreate)
        {
            if (reviewForCreate.ReviewItems == null || !reviewForCreate.ReviewItems.Any())
            {
                ModelState.AddModelError("ReviewItems", "Error! You must include at least one device to review.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Create a new Review entity
            var review = new Review
            {
                ReviewTitle = reviewForCreate.ReviewTitle,
                DateOfReview = reviewForCreate.DateOfReview,
                CustomerCountry = reviewForCreate.CustomerCountry,
                CustomerId = reviewForCreate.CustomerId,
                ReviewItems = new List<ReviewItem>()
            };

            // Process each ReviewItem and add to the Review
            foreach (var item in reviewForCreate.ReviewItems)
            {
                // Eagerly load the Device and its Model
                var device = await _context.Devices
                    .Include(d => d.Model) // Ensure Model is included
                    .FirstOrDefaultAsync(d => d.Id == item.DeviceId);

                if (device == null)
                {
                    ModelState.AddModelError("ReviewItems", $"Error! Device with Id {item.DeviceId} does not exist in the database.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                review.ReviewItems.Add(new ReviewItem
                {
                    DeviceId = device.Id,
                    Comments = item.Comments,
                    Rating = item.Rating
                });
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Map the created review to ReviewConfirmationDTO
            var createdReviewDto = new ReviewConfirmationDTO
            {
                ReviewId = review.ReviewId, // included the ReviewId
                ReviewTitle = review.ReviewTitle,
                DateOfReview = review.DateOfReview,
                CustomerCountry = review.CustomerCountry,
                CustomerId = review.CustomerId,
                OverallRating = review.ReviewItems.Average(ri => ri.Rating),
                ReviewItems = review.ReviewItems.Select(ri => new ReviewItemConfirmationDTO
                {
                    DeviceId = ri.Device.Id, 
                    DeviceName = ri.Device.Name,
                    Rating = ri.Rating,
                    Comments = ri.Comments,
                    Color = ri.Device.Color,
                    Model = ri.Device.Model.NameModel
                }).ToList()
            };

            return CreatedAtAction("GetReview", new { id = review.ReviewId }, createdReviewDto);
        }


        [HttpGet("confirmation/{id}")]
        [ProducesResponseType(typeof(ReviewConfirmationDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReviewConfirmation(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.ReviewItems)
                    .ThenInclude(ri => ri.Device) // Include Device in ReviewItems
                        .ThenInclude(d => d.Model) // Include Model in Device
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }

            var reviewConfirmation = new ReviewConfirmationDTO
            {
                ReviewTitle = review.ReviewTitle,
                DateOfReview = review.DateOfReview,
                CustomerCountry = review.CustomerCountry,
                CustomerId = review.CustomerId,
                OverallRating = review.ReviewItems.Any() ? review.ReviewItems.Average(ri => ri.Rating) : 0,
                ReviewItems = review.ReviewItems.Select(ri => new ReviewItemConfirmationDTO
                {
                    DeviceName = ri.Device.Name,
                    Rating = ri.Rating,
                    Comments = ri.Comments,
                    Color = ri.Device.Color,
                    Model = ri.Device.Model?.NameModel ?? "Unknown Model" // Handle possible null
                }).ToList()
            };

            return Ok(reviewConfirmation);
        }
    }
}
