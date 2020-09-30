using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winkeltje.Models
{
    public class HomeViewModel
    {
        public Domain.HomeImage homeImage { get; set; }
        public IFormFile ImageFile { get; set; }

    }
}
