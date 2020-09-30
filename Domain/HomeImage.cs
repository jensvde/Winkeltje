using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class HomeImage
    {
        [Key]
        public int HomeImageId { get; set; }
        public byte[] ImageData { get; set; }
        public string Text { get; set; }
    }
}
