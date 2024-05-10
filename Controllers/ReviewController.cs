using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers;


[ApiController]
[Route("/api/reviews")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;
    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllReviews([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var reviews = await _reviewService.GetAllReviewService(currentPage, pageSize);
        if (reviews.TotalCount < 1)
        {
            return ApiResponse.NotFound("No Reviews To Display");
        }
        return ApiResponse.Success<IEnumerable<Review>>(
            reviews.Items,
            "Reviews are returned successfully");
    }


    [AllowAnonymous]
    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReview(string reviewId)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            return ApiResponse.BadRequest("Invalid review ID Format");
        }

        var review = await _reviewService.GetReviewById(reviewIdGuid);
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


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview(Review newReview)
    {
        var createdReview = await _reviewService.CreateReviewService(newReview);
        if (createdReview != null)
        {
            return ApiResponse.Created(createdReview, "Review is created successfully");
        }
        else
        {
            return ApiResponse.ServerError("Error when creating new review");
        }
    }


    [Authorize]
    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(string reviewId, Review updateReview)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            return ApiResponse.BadRequest("Invalid review ID Format");
        }

        var review = await _reviewService.UpdateReviewService(reviewIdGuid, updateReview);
        if (review == null)
        {
            return ApiResponse.NotFound("No Review Founded To Update");
        }
        return ApiResponse.Success(
            review,
            "Review Is Updated Successfully"
        );
    }


    [Authorize]
    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(string reviewId)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            return BadRequest("Invalid review ID Format");
        }

        var result = await _reviewService.DeleteReviewService(reviewIdGuid);
        if (!result)
        {
            return ApiResponse.NotFound("The Review is not found to be deleted");
        }
        return ApiResponse.Success(" Review is deleted successfully");
    }
}
