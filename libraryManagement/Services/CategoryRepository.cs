using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private LibraryDbContext  _categoryContext;

        public CategoryRepository(LibraryDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public bool CategoryExists(int CategoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == CategoryId);
        }

        public ICollection<Book> GetAllBooksForACategory(int categoryId)
        {
            return _categoryContext.BookCategories.Where(c => c.CategoryId == categoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
           return _categoryContext.Categories.OrderBy(c =>c.Name).ToList();           
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public ICollection<Category> GetAllCategoriesForABook(int BookId)
        {
            return _categoryContext.BookCategories.Where(b => b.BookId == BookId).Select(c => c.Category).ToList();
        }
    }
}
