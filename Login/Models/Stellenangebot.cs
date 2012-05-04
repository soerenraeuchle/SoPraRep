using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace Login.Models
{
    //Test
    /// <summary>
    /// Die Klasse Stellenangebote repräsentiert die Tabelle "Stellenangebote" in der Datenbank
    /// </summary>
    public class Stellenangebot
    {

        public int id { get; set; }

        //---------------Grunddaten Stellenangebot--------------------------------------

        [Required]
        [StringLength(50)]
        public string stelllenName { get; set; }

        [StringLength(50)]
        public string ort { get; set; }

        [Required]
        public string beschreibung { get; set; }

        public string vorraussetzungen { get; set; }

        [Required]
        [Integer]
        public int monatsStunden { get; set; }

        [Required]
        [Integer]
        public int anzahlOffeneStellen { get; set; }
        

        //---------------Veranstalter Daten--------------------------------------

        [Required]
        [StringLength(50)]
        public string institut { get; set; }

        [Required]
        [StringLength(50)]
        public string anbieter { get; set; }

        //---------------Zeitangaben--------------------------------------

        [Required]
        public  Date startAnstellung { get; set; }

        [Required]
        public Date endeAnstellung { get; set; }

        [Required]
        public Date bewerbungsFrist { get; set; }

        

        

        

        
    }
}