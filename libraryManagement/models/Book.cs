using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(10,MinimumLength =3,ErrorMessage ="ISBN must be between 3 and 10 characters")]
        public string Isbn { get; set; }
        [MaxLength(200,ErrorMessage ="Title length can not be greater thab 200 characters")]
        public string Title { get; set; }
        public DateTime DatePublished { get; set; }
        public virtual ICollection<Review> reviews { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }

    }
}
