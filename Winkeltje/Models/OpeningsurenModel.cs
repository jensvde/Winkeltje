using System.Collections.Generic;
using Domain;

namespace Winkeltje.Models
{
    public class OpeningsurenModel
    {
        public IList<OpeningsUur> OpeningsUren { get; set; }
        public Vakantie Vakantie { get; set; }
        public bool HeeftVakantie { get; set; }
        public bool IsGeopend { get; set; }
        public OpeningsUur huidigOpeningsuur { get; set; }
    }
}