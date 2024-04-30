using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Services;

public class ReviewService
{
    private readonly AppDbContext _dbContext;

    public ReviewService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<Review>> GetAllReviewService()
    {
        return await _dbContext.Reviews.ToListAsync();
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
