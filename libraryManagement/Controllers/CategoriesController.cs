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
        //api/categories/id
        [HttpGet("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategories().Where(c => c.Id == categoryId).FirstOrDefault();


            return Ok (category);
        }


    }
}
