using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Helpers;

namespace api.Controllers;

[ApiController]
[Route("/api/reviews")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _dbContext;
    public ReviewController(ReviewService reviewService)
    {
        _dbContext = reviewService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        try
        {
            var reviews = await _dbContext.GetAllReviewService();
            if (reviews.ToList().Count < 1)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Reviews To Display"
                });
            }
            return Ok(new SuccessMessage<IEnumerable<Review>>
            {
                Message = "Reviews are returned succeefully",
                Data = reviews
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not return the Review list");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReview(string reviewId)
    {
        try
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return BadRequest("Invalid review ID Format");
            }
            var review = await _dbContext.GetReviewById(reviewIdGuid);
            if (review == null)
            {
                return NotFound(new ErrorMessage
                {
                    Message = $"No Review Found With ID : ({reviewIdGuid})"
                });
            }
            else
            {
                return Ok(new SuccessMessage<Review>
                {
                    Success = true,
                    Message = "Review is returned succeefully",
                    Data = review
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not return the Review");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }

    }


    [HttpPost]
    public async Task<IActionResult> CreateReview(Review newReview)
    {
        try
        {
            var createdReview = await _dbContext.CreateReviewService(newReview);
            if (createdReview != null)
            {
                return CreatedAtAction(nameof(GetReview), new { reviewId = createdReview.ReviewId }, createdReview);
            }
            return Ok(new SuccessMessage<Review>
            {
                Message = "Review is created succeefully",
                Data = createdReview
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not create new Review");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(string reviewId, Review updateReview)
    {
        try
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return BadRequest("Invalid review ID Format");
            }
            var review = await _dbContext.UpdateReviewService(reviewIdGuid, updateReview);
            if (review == null)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Review To Founed To Update"
                });
            }
            return Ok(new SuccessMessage<Review>
            {
                Message = "Review Is Updated Succeefully",
                Data = review
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not update the Review ");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(string reviewId)
    {
        try
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return BadRequest("Invalid review ID Format");
            }
            var result = await _dbContext.DeleteReviewService(reviewIdGuid);
            if (!result)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "The Review is not found to be deleted"
                });
            }
            return Ok(new { success = true, message = " Review is deleted succeefully" });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , the Review can not deleted");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }

}
