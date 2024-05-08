using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

namespace api.Services;

public class ReviewService
{
    private readonly AppDbContext _dbContext;
    public ReviewService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PaginationResult<Review>> GetAllReviewService(int currentPage , int pageSize)
    {
        var totalReviewCount = await _dbContext.Reviews.CountAsync();
        var review = await _dbContext.Reviews
        .Skip((currentPage -1) * pageSize)
        .Take(pageSize)
        .ToListAsync(); 
        return new PaginationResult<Review>{
          Items = review,
          TotalCount = totalReviewCount,
          CurrentPage = currentPage,
          PageSize = pageSize,
        };
    }


    public async Task<Review?> GetReviewById(Guid reviewId)
    {
        return await _dbContext.Reviews.FindAsync(reviewId);
    }


    public async Task<Review> CreateReviewService(Review newReview)
    {
        newReview.ReviewId = Guid.NewGuid();
        newReview.ReviewDate = DateTime.UtcNow;
        _dbContext.Reviews.Add(newReview);
        await _dbContext.SaveChangesAsync();
        return newReview;
    }


    public async Task<Review?> UpdateReviewService(Guid reviewId, Review updateReview)
    {
        var existingReview = await _dbContext.Reviews.FindAsync(reviewId);
        if (existingReview != null)
        {
            existingReview.Comment = updateReview.Comment ?? existingReview.Comment;
            existingReview.Status = updateReview.Status ?? existingReview.Status;
            await _dbContext.SaveChangesAsync();
        }
        return existingReview;
    }


    public async Task<bool> DeleteReviewService(Guid reviewId)
    {
        var reviewToRemove = await _dbContext.Reviews.FindAsync(reviewId);
        if (reviewToRemove != null)
        {
            _dbContext.Reviews.Remove(reviewToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

}
