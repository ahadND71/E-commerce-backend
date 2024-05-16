using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;
using SendGrid.Helpers.Errors.Model;

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


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllReviews([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var reviews = await _reviewService.GetAllReviewService(currentPage, pageSize);
        int totalCount = await _reviewService.GetTotalReviewCount();

        if (totalCount < 1)
        {
            throw new NotFoundException("No Reviews To Display");
        }

        return ApiResponse.Success(
            reviews,
            "Reviews are returned successfully", new PaginationMeta
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount
            });
    }


    [AllowAnonymous]
    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReview(string reviewId)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            throw new ValidationException("Invalid review ID Format");
        }

        var review = await _reviewService.GetReviewById(reviewIdGuid) ?? throw new NotFoundException($"No Review Found With ID : ({reviewIdGuid})");

        return ApiResponse.Success<Review>(
            review,
            "Review is returned successfully"
        );
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview(Review newReview)
    {
        var createdReview = await _reviewService.CreateReviewService(newReview) ?? throw new Exception("Error when creating new review");

        return ApiResponse.Created(createdReview, "Review is created successfully");
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(string reviewId, ReviewDto updateReview)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            throw new ValidationException("Invalid review ID Format");
        }

        var review = await _reviewService.UpdateReviewService(reviewIdGuid, updateReview) ?? throw new NotFoundException("No Order Founded To Update");

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
            throw new ValidationException("Invalid review ID Format");
        }

        var result = await _reviewService.DeleteReviewService(reviewIdGuid);
        if (!result)
        {
            throw new NotFoundException("The Review is not found to be deleted");
        }

        return ApiResponse.Success(" Review is deleted successfully");
    }
}