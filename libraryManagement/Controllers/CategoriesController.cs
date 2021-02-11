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
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetAllBooksForACategory(int categoryId)
        {
            var booksForCategory = _categoryRepository.GetAllBooksForACategory(categoryId).ToList();
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var booksDto = new List<BookDto>();
            foreach (var books in booksForCategory)
            {
                booksDto.Add(new BookDto
                {
                    Id = books.Id,
                    Title = books.Title,
                    Isbn = books.Isbn,
                    DatePublished = books.DatePublished

                });
            }
            return Ok(booksDto);
        }
        //POST API For Category
        //api/categories
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType (422)]
        public IActionResult CreateCategory([FromBody] Category categoryToCreate)
        {
            //Validation
            //check if the categoryToCreate is exist on body. If it is null then it is a bad request
            if (categoryToCreate == null)
                return BadRequest(ModelState);
           
            //If body has category then check is it already exist or not. If it returns not null mean it exist then
            //through custom error that category already exist.
            var category = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", $"Category {categoryToCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }
          
            //Check if the state valid or not. If it is not valid then through error. Bad request.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //last check usethe CreateCategory method and check if category created in DB or not. If not then through
            //custom status code 500 that system has some issue.
            if (!_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {categoryToCreate.Name}");
                return StatusCode(500, ModelState);
            }
            //if category created  then return the new created category.
            return CreatedAtRoute("GetCategory", new { categoryId = categoryToCreate.Id }, categoryToCreate);
        }
        //api/categories/{categoryId}
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]             
        public IActionResult UpdateCategory(int categoryId,[FromBody] Category categoryToUpdate)
        {
            if (categoryToUpdate == null)
                return BadRequest(ModelState);
            //check category id from body exist in database
            if (categoryId != categoryToUpdate.Id)
                return NotFound();
            //check the category name is exist in database then it will be error

            if (_categoryRepository.IsDuplicateCategoryName(categoryId, categoryToUpdate.Name))
            {
                ModelState.AddModelError("", $"Category {categoryToUpdate.Name} already exists");
                return StatusCode(422, ModelState);
            }
            //check if model exist
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //check if item is updated or not. If not then through error code 500
            if (_categoryRepository.UpdateCategory(categoryToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {categoryToUpdate.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();                     
        }               
           
    }
}
