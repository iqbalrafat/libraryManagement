﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.models
{
    public class Reviewer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="First Name can not be more than 100 characters")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "First Name can not be more than 200 characters")]
        public string LastName { get; set; }

        //add navigational property
        public virtual ICollection<Review> Reviews{ get; set; }

    }
}
