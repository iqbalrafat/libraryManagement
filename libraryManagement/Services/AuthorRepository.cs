﻿using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private LibraryDbContext _authorContext;
        public AuthorRepository(LibraryDbContext authorContext)
        {
            _authorContext = authorContext;
        }
        public bool AuthorExists(int authorId)
        {
            return _authorContext.Authors.Any(a => a.Id == authorId);
        }

        public bool CreateAuthor(Author author)
        {
            _authorContext.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _authorContext.Remove(author);
            return Save();
        }

        public Author GetAuthor(int authorId)
        {
            return _authorContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.OrderBy(a => a.FirstName).ToList();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return _authorContext.BookAuthors.Where(b => b.BookId == bookId).Select(a =>a.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _authorContext.BookAuthors.Where(a => a.AuthorId == authorId).Select(b => b.Book).ToList();
        }

        public bool Save()
        {
            var saved = _authorContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _authorContext.Update(author);
            return Save();
        }
    }
}
