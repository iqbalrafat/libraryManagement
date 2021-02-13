using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class BookRepository:IBookRepository
    {
        LibraryDbContext _bookContext;
        public BookRepository(LibraryDbContext bookContext)
        {
            _bookContext = bookContext;
        }

        public bool BookExistsById(int bookId)
        {
            return _bookContext.Books.Any(b => b.Id == bookId);
        }

        public bool BookExistsByIsbn(string bookIsbn)
        {
            return _bookContext.Books.Any(b => b.Isbn==bookIsbn);
        }

        public bool CreateBook(List<int> authorId, List<int> categoriesId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBook(Book book)
        {
            _bookContext.Remove(book);
            return Save();
        }

        public Book GetBookById(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBookByIsbn(string bookIsbn)
        {
            return _bookContext.Books.Where(b => b.Isbn==bookIsbn).FirstOrDefault();
        }

        public decimal GetBookRating(int bookId)
        {
            var reviews= _bookContext.Reviews.Where(r => r.Book.Id == bookId);
            if (reviews.Count() <= 0)
                return 0;
            return ((decimal)reviews.Sum(r => r.Rating / reviews.Count()));
        }

        public ICollection<Book> GetBooks()
        {
            return _bookContext.Books.OrderBy(b => b.Title).ToList();
        }

        public bool IsDuplicateIsbn(int bookId, string bookIsbn)
        {
            var book= _bookContext.Books.Where(b => b.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() && b.Id == bookId).FirstOrDefault();
            return book == null ? false : true;
        }

        public bool Save()
        {
            var saved = _bookContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateBook(List<int> authorId, List<int> categoriesId)
        {
            throw new NotImplementedException();
        }
    }
}
