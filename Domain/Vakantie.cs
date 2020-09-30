using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Domain
{
    public class Vakantie
    {
        [Key]
        public int VakantieId { get; set; }
        public DateTime startDatum { get; set; }
        public DateTime eindDatum { get; set; }
        public string reden { get; set; }

        public bool HeeftVakantie(DateTime dateTime)
        {
            return dateTime < eindDatum && dateTime > startDatum;
        }

        public string GetVakantieString()
        {
          //  var culture = new CultureInfo( "nl-BE" );
           // DateTime startLocal = DateTime.Parse(startDatum.ToString(), CultureInfo.GetCultureInfo("nl-BE"));
           // DateTime EndLocal = DateTime.Parse(eindDatum.ToString(), CultureInfo.GetCultureInfo("nl-BE"));
            string start = String.Format("{0:dddd d MMMM}", startDatum);
            string eind = String.Format("{0:dddd d MMMM}", eindDatum);
            return "Wij zijn met vakantie (" + reden + ")" + ". De winkel is gesloten van " + start.ToLower() + " tot en met " + eind.ToLower() + ".";
        }
    }
}