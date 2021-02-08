using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);

        ICollection<Category> GetAllCategoriesForABook(int BookId);
        ICollection<Book> GetAllBooksForACategory(int categoryId);

        bool CategoryExists(int CategoryId);
        bool IsDuplicateCategoryName(int CategoryId, string CategoryName);

    }
}
