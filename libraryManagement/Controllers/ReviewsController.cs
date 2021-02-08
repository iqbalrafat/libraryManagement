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
    }
}
