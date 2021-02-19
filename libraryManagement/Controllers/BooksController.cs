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
    public class BooksController: Controller
    {
        //we add the category, author and review repoitories as book has relationship with them. To delete book
        //we need to delet the relation items too.
        private IBookRepository _bookRepository;
        private IAuthorRepository _authorRepository;
        private ICategoryRepository _categoryRepository;
        private IReviewRepository _reviewRepository;
        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository,
            IReviewRepository reviewRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetBooks();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var booksDto = new List<BookDto>();
            foreach(var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id=book.Id,
                    Title=book.Title,
                    Isbn=book.Isbn,
                    DatePublished=book.DatePublished
                });
            }
            return Ok(booksDto);
        }
        //api/bOOKS/{bookId}
        [HttpGet("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookById(int bookId)
        {
            if (!_bookRepository.BookExistsById(bookId))
                return NotFound();
            var book = _bookRepository.GetBookById(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bookDto = new BookDto()
            {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
             };
            
            return Ok(bookDto);
        }
        //api/books/{bookId}/rating
        [HttpGet("{bookId}/rating")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public IActionResult GetBookRating(int bookId)
        {
            if (!_bookRepository.BookExistsById(bookId))
                return NotFound();
            var rating = _bookRepository.GetBookRating(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }

        //Perform all validation before performing delete books
        private StatusCodeResult ValidateBook(List<int> authId, List<int> catId,Book book)
        {
            //valiadate if book is empty or autor or category are missing
            if(book==null ||authId.Count<=0 || catId.Count <= 0)
            {
                ModelState.AddModelError("", "Missing book, author or category");
                return BadRequest();
            }
            //check the duplicate ISBN
            if (_bookRepository.IsDuplicateIsbn(book.Id, book.Isbn))
            {
                ModelState.AddModelError("", "Duplicate ISBN");
                    return StatusCode(422);
            }
            //check the author exist. if not then through error
            foreach(var id in authId)
            {
                if (!_authorRepository.AuthorExists(id))
                {
                    ModelState.AddModelError("", "Author not found");
                        return StatusCode(404);
                }
            }
            //check the existence of Category
            foreach(var id in catId)
            {
                if (!_categoryRepository.CategoryExists(id))
                {
                    ModelState.AddModelError("", "Category does not found");
                    return StatusCode(404);
                }
            }
            
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Critical Error");
                return BadRequest();
            }

            return NoContent();
        }
    }
}


