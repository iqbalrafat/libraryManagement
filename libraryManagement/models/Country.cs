using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Name can not be more than 50 characters")]
        public int Name { get; set; }
        //add navigational property
        public virtual ICollection<Author> Authors {get;set;}  

    }
}
