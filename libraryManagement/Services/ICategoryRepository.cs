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

        Category GetCategoryOfABook(int bookId);
        ICollection<Book> GetBooksForACategory(int categoryId);

        bool CategoryExists(int CategoryId);

    }
}
