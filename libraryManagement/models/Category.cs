using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Name can be greater than 50 characters")]
        public string Name { get; set; }
        public virtual ICollection<BookCategory> BookCategories { get; set; }

    }

}
