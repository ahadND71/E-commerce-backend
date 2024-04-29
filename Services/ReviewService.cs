public class ReviewService
{

    public static List<Review> _reviews = new List<Review>() {
    new Review{
        ReviewId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        ProductId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        CustomerId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        OrderId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        Rating = 5,
        Comment = "Great product, thank you",
        ReviewDate = DateTime.Now,
        Status = "pending",
        IsAnonymous = false
    },
    new Review{
        ReviewId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        ProductId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        CustomerId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        OrderId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        Rating = 4,
        Comment = "Awesome purchase, recommended!",
        ReviewDate = DateTime.Now,
        Status = "approved",
        IsAnonymous = false
    },
    new Review{
        ReviewId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        ProductId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        CustomerId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        OrderId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        Rating = 4,
        Comment = "Best producted I've tried so far!",
        ReviewDate = DateTime.Now,
        Status = "approved",
        IsAnonymous = false
    }
};

    public IEnumerable<Review> GetAllReviewService()
    {
        return _reviews;
    }


    public Review? GetReviewById(Guid reviewId)
    {
        return _reviews.Find(review => review.ReviewId == reviewId);
    }


    public Review CreateReviewService(Review newReview)
    {
        newReview.ReviewId = Guid.NewGuid();
        newReview.ReviewDate = DateTime.Now;
        _reviews.Add(newReview);
        return newReview;
    }


    public Review UpdateReviewService(Guid reviewId, Review updateReview)
    {
        var existingReview = _reviews.FirstOrDefault(c => c.ReviewId == reviewId);
        if (existingReview != null)
        {
            existingReview.Comment = updateReview.Comment;
            existingReview.Status = updateReview.Status;

        }
        return existingReview;
    }


    public bool DeleteReviewService(Guid reviewId)
    {
        var reviewToRemove = _reviews.FirstOrDefault(c => c.ReviewId == reviewId);
        if (reviewToRemove != null)
        {
            _reviews.Remove(reviewToRemove);
            return true;
        }
        return false;
    }

}