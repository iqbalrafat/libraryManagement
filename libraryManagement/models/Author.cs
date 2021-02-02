using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.models
{
    public class Author
    {   [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Database generated ID. Since we define ID so we don't need to define but it is for clarification
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage message="First name cannot be more than 200 characters")]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }

    }
}
