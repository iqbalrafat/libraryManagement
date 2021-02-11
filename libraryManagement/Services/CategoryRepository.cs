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

        public bool IsDuplicateCategoryName(int CategoryId, string CategoryName)
        {
            var Category = _categoryContext.Categories.Where(c => c.Name.Trim().ToUpper() == CategoryName.Trim().ToUpper() && c.Id == CategoryId).FirstOrDefault();
            return Category == null ? false : true;
        }

        public bool CreateCategory(Category category)
        {
            _categoryContext.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
             _categoryContext.Remove(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _categoryContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
