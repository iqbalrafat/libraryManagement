using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int authorId);
        ICollection <Author> GetAuthorsOfABook(int bookId);
        ICollection<Book> GetBooksByAuthor(int authorId);
        bool AuthorExists(int authorId);

    }
}
