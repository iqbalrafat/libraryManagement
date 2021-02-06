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
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        //api/categories
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type =typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return Ok(categoriesDto);
        }
        //api/categories/{categoryId
        [HttpGet("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var category = _categoryRepository.GetCategory(categoryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryDto = new CategoryDto()  // creating object CategoryDto, Only value that we need
            {
                Id = category.Id,
                Name = category.Name
            };             
            return Ok(categoryDto);
        }
        //api/categories/books/{bookId}
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllCategoriesForABook(int BookId)
        {
            var categoriesForBook = _categoryRepository.GetAllCategoriesForABook(BookId).ToList();
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var categoriesDto = new List<CategoryDto>();
            foreach ( var category in categoriesForBook)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return Ok(categoriesDto);
        }
        //api/categories/{categoryId}/books
        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllBooksForACategory(int categoryId)
        {
            var booksForCategory = _categoryRepository.GetAllBooksForACategory(categoryId).ToList();
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var categoriesDto = new List<CategoryDto>();
            foreach (var books in booksForCategory)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = books.Id,
                    Name = books.Title
                });
            }
            return Ok(categoriesDto);
        }
    }
}
