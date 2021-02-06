using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    interface IReviewerRepository
    {
        //to get the list of all Reviewers
        ICollection<Reviewer> GetReviewers();
        //to get the specific Reviewer
        Reviewer GetReviewer(int reviewerId);

        //To Get all Reviews from a specific Reviewer
        ICollection <Review> GetReviewesByReviewer(int reviewerId);
        
        //To Get Reviewer for a specific Review
        Reviewer GetReviewerOfAReview(int ReviewId);
        
        //Check the existence of a Reviewer
        bool ReviewerExists(int reviewrId);
    }
}
