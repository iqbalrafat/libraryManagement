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
        public bool CreateBook(List<int> authorId, List<int> categoriesId, Book book)
        {
            //find the author and catgeories list

            var authors = _bookContext.Authors.Where(a => authorId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();
            //now loop through authors and categories list and create the list of the bookauthor and bookcategorynand finaly save them
            foreach(var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _bookContext.Add(bookAuthor);
            }
            foreach(var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Book = book,
                    Category = category
                };
                _bookContext.Add(bookCategory);
            }
            return Save();
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
        public bool UpdateBook(List<int> authorId, List<int> categoriesId, Book book)
        {
            //find the author and catgeories list

            var authors = _bookContext.Authors.Where(a => authorId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();
            
            var bookAuthorsToDelete = _bookContext.BookAuthors.Where(b => b.BookId == book.Id);
            var bookCategoriesToDelete =_bookContext.BookCategories.Where(b=>b.BookId==book.Id);

            _bookContext.RemoveRange(bookAuthorsToDelete);
            _bookContext.RemoveRange(bookCategoriesToDelete);

            foreach (var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _bookContext.Add(bookAuthor);
            }
            foreach (var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Book = book,
                    Category = category
                };
                _bookContext.Update(bookCategory);
            }
            return Save();
        }
    }
}
