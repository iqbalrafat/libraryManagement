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
        [HttpGet]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();

            return Ok(categories);
        }

    }
}
