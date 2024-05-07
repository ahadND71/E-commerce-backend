using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using api.Authentication.Identity;

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


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        try
        {
            var reviews = await _dbContext.GetAllReviewService();
            if (reviews.ToList().Count < 1)
            {
                return ApiResponse.NotFound("No Reviews To Display");
            }
            return ApiResponse.Success<IEnumerable<Review>>(
                reviews,
                "Reviews are returned successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Review list");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [AllowAnonymous]
    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReview(string reviewId)
    {
        try
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return ApiResponse.BadRequest("Invalid review ID Format");
            }
            var review = await _dbContext.GetReviewById(reviewIdGuid);
            if (review == null)
            {
                return ApiResponse.NotFound(
                    $"No Review Found With ID : ({reviewIdGuid})");
            }
            else
            {
                return ApiResponse.Success<Review>(
                  review,
                  "Review is returned successfully"
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Review");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPost]
    public async Task<IActionResult> CreateReview(Review newReview)
    {
        try
        {
            var createdReview = await _dbContext.CreateReviewService(newReview);
            if (createdReview != null)
            {
                return ApiResponse.Created<Review>(createdReview, "Review is created successfully");
            }
            else
            {
                return ApiResponse.ServerError("Error when creating new review");

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Review");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(string reviewId, Review updateReview)
    {
        try
        {
            if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
            {
                return ApiResponse.BadRequest("Invalid review ID Format");
            }
            var review = await _dbContext.UpdateReviewService(reviewIdGuid, updateReview);
            if (review == null)
            {
                return ApiResponse.NotFound("No Review Founded To Update");
            }
            return ApiResponse.Success<Review>(
                review,
                "Review Is Updated Successfully"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot update the Review ");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
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

                return ApiResponse.NotFound("The Review is not found to be deleted");
            }
            return ApiResponse.Success(" Review is deleted successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, the Review can not deleted");
            return ApiResponse.ServerError(ex.Message);

        }
    }
}
