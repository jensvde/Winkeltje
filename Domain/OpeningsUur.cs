using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    public class OpeningsUur
    {
        [Key]
        public int OpeningsUurId { get; set; }

        public string Dag { get; set; } = "";
        public string DagVanDeWeek { get; set; }
        public string StartVoormiddag { get; set; } = "";
        public string EindVoormiddag { get; set; }= "";
        public string StartNamiddag { get; set; }= "";
        public string EindNamiddag { get; set; }= "";
        public bool SluitingsDag { get; set; }
        public bool SluitingsHalfDag { get; set; }

        public string getVoormiddagString(string dayOfWeek)
        {
            string toReturn = "";
            switch (dayOfWeek)
            {
                case "Maandag": toReturn = StartVoormiddag + " - " + EindVoormiddag;
                    break;
                case "Dinsdag": toReturn = "Gesloten";
                    break;
                case "Woensdag": toReturn = StartVoormiddag + " - " + EindVoormiddag;
                    break;
                case "Donderdag": toReturn = StartVoormiddag + " - " + EindVoormiddag;
                    break;
                case "Vrijdag": toReturn = StartVoormiddag + " - " + EindVoormiddag;
                    break;
                case "Zaterdag": toReturn = StartVoormiddag;
                    break;
                case "Zondag": toReturn = StartVoormiddag + " - " + EindVoormiddag;;
                    break;
            }

            return toReturn;
        }
        
        public string getNamiddagString(string dayOfWeek)
        {
            string toReturn = "";
            switch (dayOfWeek)
            {
                case "Maandag": toReturn = StartNamiddag + " - " + EindNamiddag;
                    break;
                case "Dinsdag": toReturn = "Gesloten";
                    break;
                case "Woensdag": toReturn = StartNamiddag + " - " + EindNamiddag;
                    break;
                case "Donderdag": toReturn = "Gesloten";
                    break;
                case "Vrijdag": toReturn = StartNamiddag + " - " + EindNamiddag;
                    break;
                case "Zaterdag": toReturn = EindNamiddag;
                    break;
                case "Zondag": toReturn = "Gesloten";
                    break;
            }

            return toReturn;
        }

        public bool isGeopend(DateTime huidigeTijd)
        {
            if (SluitingsDag)
                                 {
                                     return false;
                                 }  
            string toParse = EindNamiddag.Split('.')[0];

                     if (SluitingsHalfDag)
                     {
                         toParse = EindVoormiddag.Split('.')[0];
                     }
            DateTime startVoor = new DateTime(1,1,2,Int32.Parse(StartVoormiddag.Split('.')[0].ToString()),0,0);
            //DateTime eindVoor = new DateTime(1,1,2,Int32.Parse(EindVoormiddag),0,0);
            //DateTime startNa = new DateTime(1,1,2,Int32.Parse(StartVoormiddag),0,0);
            

            DateTime eindNa = new DateTime(1,1,2,Int32.Parse(toParse),0,0);
            huidigeTijd = new DateTime(1,1,2,huidigeTijd.Hour,huidigeTijd.Minute,0);
            
            return huidigeTijd < eindNa && huidigeTijd > startVoor;
        }
    }
}