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
    public class AuthorsController:Controller
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }
        //api/Authors
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAuthors();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           var  authorsDto = new List<AuthorDto>();
            foreach(var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok (authorsDto);
        }
        //api/Authors/{authorId}
        [HttpGet("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();
            var author = _authorRepository.GetAuthor(authorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorDto = new AuthorDto()
            {
                id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
            return Ok(authorDto);
        }
        //api/Authors/books/{bookId}
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthorsOfABook(int bookId)
        {
            if (!_bookRepository.BookExistsById(bookId))
                return NotFound();
            var authors = _authorRepository.GetAuthorsOfABook(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorsDto = new List<AuthorDto>();
            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDto);
        }
        //api/Authors/{authorId}/books
        [HttpGet("{authorId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();
            var books = _authorRepository.GetBooksByAuthor(authorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var booksDto = new List<BookDto>();
            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished=book.DatePublished
                });
            }
            return Ok(booksDto);
        }

     }
}
