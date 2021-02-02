using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200,MinimumLength =10,ErrorMessage ="Headline should be minimum 10 and maximum 200 Characters")]
        public string Headline { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 50, ErrorMessage = "Reviews should be minimum 50 and maximum 2000 Characters")]
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual Book Book { get; set; }

    }
}
