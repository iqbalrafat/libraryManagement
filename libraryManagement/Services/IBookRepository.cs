using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    interface IBookRepository
    {
        ICollection<Book> GetBooks();
        Book GetBookById (int bookId);
        Book GetBookByIsbn(string bookIsbn);        
        decimal GetBookRating(int bookId);     
        bool BookExistsById(int bookId);
        bool BookExistsByIsbn(string bookIsbn);
        bool IsDuplicateIsbn(int bookId, string bookIsbn);
    }
}
