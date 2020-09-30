using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Winkeltje.Models
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Naam { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Bericht { get; set; }
    }
}
