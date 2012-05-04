using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    //Test
    /// <summary>
    /// Die Klasse Bewerbung beschreibt eine Hiwi Bewerbung für ein Stellenangebot. 
    /// Dabei werden die Benutzerdaten mit einem Stellenangebot verknüpft.
    /// </summary>
    public class Bewerbung
    {
        [Integer]
        [Required]
        public int id { get; set; }

        //Stellenangebots ID zur eindeutigen referenzierung der Bewerbung an dein Stellenangebot
        [Integer]
        [Required]
        public int stellenangebotID { get; set; }


        //Benutzer ID zur eindeutigen referenzierung eines Hiwis für ein Stellenangebot
        [Integer]
        [Required]
        public int benutzerID { get; set; }

        //Hiwi Kenntnisse für die Bewerbung (Null werte sind zugelassen)
        public string kenntnisse { get; set; }


        //Status beschreibt den Bearbeitungsgrad der Bewerbung
        [Integer]
        [Required]
        public int status { get; set; }
    }
}