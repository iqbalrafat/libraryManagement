using libraryManagement.DTos;
using libraryManagement.models;
using libraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController:Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;
        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }
        //api/reviewers
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type =typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewers()
        {
           var Reviewers = _reviewerRepository.GetReviewers().ToList();
           if (!ModelState.IsValid)
               return BadRequest(ModelState);
                    var ReviewersDto = new List<ReviewerDto>();
                    foreach(var reviewer in Reviewers)
                    {
                        ReviewersDto.Add(new ReviewerDto
                        {
                            Id = reviewer.Id,
                            FirstName = reviewer.FirstName,
                            LastName = reviewer.LastName
                        });
                    }
               return Ok(ReviewersDto);
        }
        //api/Reviewers/{ReviewerId}
        [HttpGet("{ReviewerId}",Name ="GetReviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type=typeof(ReviewerDto))]
        public IActionResult GetReviewer(int ReviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(ReviewerId))
                return NotFound();
            var reviewer = _reviewerRepository.GetReviewer(ReviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName,
            };
            return Ok(reviewerDto);
        }
        //api/reviewers/{reviewerId}/reviews
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type =typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var reviews = _reviewerRepository.GetReviewesByReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);           
            var reviewsDto = new List<ReviewDto>();
            foreach(var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });                
            }
            return Ok(reviewsDto);
        }
        //api/reviewers/{reviewId}/reviewer
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type =typeof(ReviewerDto))]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
               return NotFound();
            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerDto = new ReviewerDto
            {
                Id=reviewer.Id,
                FirstName=reviewer.FirstName,
                LastName=reviewer.LastName
            };
            return Ok(reviewerDto);
        }
        //CRUD Operation Method
        //api/reviewers
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201,Type=typeof(Reviewer))]

        public IActionResult CreateReviewer([FromBody] Reviewer reviewerToCreate)
        {
            if (reviewerToCreate == null)
                return BadRequest(ModelState);

            var reviewer = _reviewerRepository.GetReviewers().Where(c => c.FirstName.Trim().ToUpper() == reviewerToCreate.FirstName.Trim().ToUpper()).FirstOrDefault();
            if(reviewer!=null)
            {
                ModelState.AddModelError("", $"Country {reviewerToCreate.FirstName} already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_reviewerRepository.CreateReviewer(reviewerToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving"+
                    $"{reviewerToCreate.FirstName}{reviewerToCreate.LastName}");
                    return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReviewer",new { reviewerId = reviewerToCreate.Id },reviewerToCreate);
        }


    }
}
