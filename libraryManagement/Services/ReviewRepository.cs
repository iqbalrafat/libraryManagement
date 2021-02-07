﻿using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private LibraryDbContext _reviewContext;
        public ReviewRepository(LibraryDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }
        public Book GetBookOfAReview(int reviewId)
        {
            var bookId = _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(r => r.Book.Id).FirstOrDefault();
            return _reviewContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewesOfABook(int bookId)
        {
            return _reviewContext.Reviews.Where(r => r.Book.Id == bookId).ToList();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.OrderBy(r => r.Rating).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == reviewId);
        }
    }
}