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
        [ProducesResponseType(200)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();

            return Ok(categories);
        }
        //api/categories/{categoryId
        [HttpGet("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategories().Where(c => c.Id == categoryId).FirstOrDefault();
            return Ok(category);
        }
        //api/categories/books/{bookId}
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetAllCategoriesForABook(int BookId)
        {
            var categoriesForBook = _categoryRepository.GetAllCategoriesForABook(BookId).ToList();
            return Ok(categoriesForBook);
        }
        //api/categories/{categoryId}/books
        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetAllBooksForACategory(int categoryId)
        {
            var booksForCategory = _categoryRepository.GetAllBooksForACategory(categoryId).ToList();
            return Ok(booksForCategory);
        }


    }
}
