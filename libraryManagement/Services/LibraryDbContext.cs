using libraryManagement.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class LibraryDbContext:DbContext
    {
        //since we inherted the DbContext se we need to define a constructor.
        public LibraryDbContext(DbContextOptions<DbContext> options)
            :base(options)
        {
            // heer we define the patern we like to create when EF run. In our case we like migration.
            Database.Migrate();
        }
        //Define DBSet
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Reviewer> Reviewers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<BookCategory> BookCategories { get; set; }

        //onModelCreating method is part of Dbcontext, it takes n argument of type ModelBuilder which allow to specify the
        //relationship between entities. to create a method we override it from dbcontext
        
        
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            //Make a relationship between Book and category. first we check one book with multiple categories then one category with multiple book
            modelBuilder.Entity<BookCategory>()
                   .HasKey(bc => new { bc.BookId, bc.CategoryId }); //bc for BookCategory we define the BookId and CategoryId
            modelBuilder.Entity<BookCategory>()
                .HasOne(b => b.Book)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(b => b.BookId);
            modelBuilder.Entity<BookCategory>()
                .HasOne(c => c.Category)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(c => c.CategoryId);
            modelBuilder.Entity<BookCategory>()
                .HasOne(b => b.Book)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(b => b.BookId);
            modelBuilder.Entity<BookCategory>()
                .HasOne(c => c.Category)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(c => c.CategoryId);
            //Make a relationship between Book and author. first we check one book with multiple categories then one category with multiple book
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId,ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)  //ba stands for BookAuthor
                .HasForeignKey(b => b.BookId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(a => a.AuthorId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(b => b.BookId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(a => a.AuthorId);














        }




    }
}
