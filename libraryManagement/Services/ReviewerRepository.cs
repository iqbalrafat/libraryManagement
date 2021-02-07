﻿using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        private LibraryDbContext _ReviewerContext;
        public ReviewerRepository(LibraryDbContext ReviewerContext)
        {
            _ReviewerContext = ReviewerContext;
        }
        public Reviewer GetReviewer(int reviewerId)
        {
            return _ReviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfAReview(int reviewId)
        {
          var reviewerId= _ReviewerContext.Reviews.Where(r => r.Id == reviewId).Select(rr => rr.Reviewer.Id).FirstOrDefault();
            return _ReviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
           
        }
        public ICollection<Reviewer> GetReviewers()
        {
            return _ReviewerContext.Reviewers.OrderBy(r => r.FirstName).ToList();
        }

        public ICollection<Review> GetReviewesByReviewer(int reviewerId)
        {
            return _ReviewerContext.Reviews.Where(r => r.Reviewer.Id==reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _ReviewerContext.Reviewers.Any(r=>r.Id==reviewerId);
        }
    }
}
