
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        public ReviewController()
        {
            _reviewService = new ReviewService();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var reviews = _reviewService.GetAllReviewService();
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        public IActionResult GetReview(string reviewId)
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return BadRequest("Invalid review ID Format");
            }
            var review = _reviewService.GetReviewById(reviewIdGuid);
            if (review == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(review);
            }

        }

        [HttpPost]
        public IActionResult CreateReview(Review newReview)
        {
            var createdReview = _reviewService.CreateReviewService(newReview);
            return CreatedAtAction(nameof(GetReview), new { reviewId = createdReview.ReviewId }, createdReview);
        }


        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(string reviewId, Review updateReview)
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return BadRequest("Invalid review ID Format");
            }
            var review = _reviewService.UpdateReviewService(reviewIdGuid, updateReview);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }


        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(string reviewId)
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return BadRequest("Invalid review ID Format");
            }
            var result = _reviewService.DeleteReviewService(reviewIdGuid);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}