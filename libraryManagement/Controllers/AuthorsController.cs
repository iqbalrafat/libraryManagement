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
    public class AuthorsController : Controller
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private ICountryRepository _countryRepository;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository, ICountryRepository countryRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _countryRepository = countryRepository;
        }
        //api/Authors
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAuthors();
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
        //api/Authors/{authorId}
        [HttpGet("{authorId}", Name = "GetAuthor")]
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
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDto);
        }
        //CRUD Operation
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Author))]
        public IActionResult CreateAuthor([FromBody] Author authorToCreate)
        {
            if (authorToCreate == null)
                return BadRequest(ModelState);
            //If we look the relationship it is clear that author is linked with country. therefore we need to check
            //the country exist or not for this first add CountryRepository and then check coutry existence

            if (!_countryRepository.CountryExist(authorToCreate.Country.Id))
            {
                ModelState.AddModelError("", $"Country Does not exist");
                return StatusCode(404, ModelState);
            }
            authorToCreate.Country = _countryRepository.GetCountry(authorToCreate.Country.Id);
            if (!_authorRepository.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("", $"something went wrong to save" +
                    $"{authorToCreate.FirstName}{authorToCreate.LastName}");
                return StatusCode(500, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToCreate.Id }, authorToCreate);
        }

        //api/authors/authorId
        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201)]

        public IActionResult UpdateAuthor(int authorId, [FromBody] Author authorToUpdate)
        {
            if (authorToUpdate == null)
                return BadRequest(ModelState);
            if (authorId != authorToUpdate.Id)
                return BadRequest(ModelState);

            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_authorRepository.UpdateAuthor(authorToUpdate))
            {
                ModelState.AddModelError("", $"something went wromg while update" +
                    $"{authorToUpdate.FirstName}{authorToUpdate.LastName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


     }
}
