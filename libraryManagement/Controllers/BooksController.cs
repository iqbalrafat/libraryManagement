using libraryManagement.DTos;
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

        private IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
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

    }
}


//public bool BookExistsById(int bookId)
//{
//    return _bookContext.Books.Any(b => b.Id == bookId);
//}

//public bool BookExistsByIsbn(string bookIsbn)
//{
//    return _bookContext.Books.Any(b => b.Isbn == bookIsbn);
//}

//public Book GetBookById(int bookId)
//{
//    return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
//}

//public Book GetBookByIsbn(string bookIsbn)
//{
//    return _bookContext.Books.Where(b => b.Isbn == bookIsbn).FirstOrDefault();
//}

//public decimal GetBookRating(int bookId)
//{
//    var reviews = _bookContext.Reviews.Where(r => r.Book.Id == bookId);
//    if (reviews.Count() <= 0)
//        return 0;
//    return ((decimal)reviews.Sum(r => r.Rating / reviews.Count()));
//}


//public bool IsDuplicateIsbn(int bookId, string bookIsbn)
//{
//    var book = _bookContext.Books.Where(b => b.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() && b.Id == bookId);
//    return book == null ? false : true;
//}