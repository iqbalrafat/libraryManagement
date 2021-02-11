using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public interface IReviewRepository
    {//to get the list of all Reviewes
        ICollection<Review> GetReviews();
        //to get the specific Review
        Review GetReview(int reviewId);

        //To Get all Reviews of a specific book
        ICollection<Review> GetReviewesOfABook(int bookId);

        //To Get Book for a specific Review
        Book GetBookOfAReview(int reviewId);

        //Check the existence of a Review
        bool ReviewExists(int reviewId);
        //CRUD Operations
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();

    }
}
