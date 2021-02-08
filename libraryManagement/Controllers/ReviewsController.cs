using libraryManagement.DTos;
using libraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Controllers
{
    [Route("api/[Controller]")]
    public class ReviewsController: Controller
    {
        private IReviewRepository _reviewRepository;
        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        //api/reviews
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var Reviews = _reviewRepository.GetReviews();
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var reviewsDto = new List<ReviewDto>();
            foreach(var review in Reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline=review.Headline,
                    ReviewText=review.ReviewText,
                    Rating=review.Rating,
                });
            }
            return Ok(reviewsDto);
        }
        //api/reviews/{reviewId}
        [HttpGet("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type =typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var review = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewDto = new ReviewDto()
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };
            return Ok(reviewDto);
        }
    }
}
