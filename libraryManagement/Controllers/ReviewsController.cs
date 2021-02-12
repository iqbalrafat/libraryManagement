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
    [Route("api/[Controller]")]
    public class ReviewsController: Controller
    {
        private IReviewRepository _reviewRepository;
        private IBookRepository _bookRepository;
        private IReviewerRepository _reviewerRepository;
        public ReviewsController(IReviewRepository reviewRepository,IBookRepository bookRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _reviewerRepository = reviewerRepository;
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
        [HttpGet("{reviewId}",Name="GetReview")]
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
        //api/reviews/Books/{bookId}
        [HttpGet("Books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviewesOfABook(int bookId)
        {
            if (!_bookRepository.BookExistsById(bookId))
                return NotFound();
            var reviews = _reviewRepository.GetReviewesOfABook(bookId);
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
        //api/reviews/{reviewId/book
        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBookOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var book = _reviewRepository.GetBookOfAReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var booksDto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                DatePublished=book.DatePublished,
                Isbn=book.Isbn
            };
            return Ok(booksDto);
        }
        //api/reviews
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Review))]
        public IActionResult CreateReview([FromBody] Review reviewToCreate)
        {
            if (reviewToCreate == null)
                BadRequest(ModelState);
            if (!_reviewerRepository.ReviewerExists(reviewToCreate.Reviewer.Id))
                ModelState.AddModelError("", "Reviewer Does not exist!");
            if (!_bookRepository.BookExistsById(reviewToCreate.Book.Id))
                ModelState.AddModelError("", "Book Does not exist!");
            if (!ModelState.IsValid)
                return StatusCode(404,ModelState);
            //creating reviewToCreate Object
            reviewToCreate.Book = _bookRepository.GetBookById(reviewToCreate.Book.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewer(reviewToCreate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_reviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReview", new { reviewId = reviewToCreate.Id }, reviewToCreate);
        }
        //api/reviews/{reviewId}
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int reviewId,[FromBody]Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);
            //Check the Review that need to update has same reviewID that is come via Body. If not then 
            //through error
            if (reviewId != reviewToUpdate.Id)
                return BadRequest(ModelState);
          
            //Chcek the review exist or not.
            if (!_reviewRepository.ReviewExists(reviewId))
                ModelState.AddModelError("", "Review does not exist");                
            //Check the reviewer exist or not
            if(!_reviewerRepository.ReviewerExists(reviewToUpdate.Reviewer.Id))
                ModelState.AddModelError("", "Reviewer does not exist");
            //check the book exist or not
            if(!_bookRepository.BookExistsById(reviewToUpdate.Book.Id))
                ModelState.AddModelError("", "Book does not exist");
            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);
           
            //creating the reviewToUpdate object by adding book and reviewer 
            reviewToUpdate.Book = _bookRepository.GetBookById(reviewToUpdate.Book.Id);
            reviewToUpdate.Reviewer = _reviewerRepository.GetReviewer(reviewToUpdate.Reviewer.Id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", "something wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //api/reviews/{reviewId}
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", $"Something wrong while delete review");
                return StatusCode(500, ModelState);
            }
            return NoContent();



            return Ok();
        }

    }
}
