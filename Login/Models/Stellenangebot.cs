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

        public Stellenangebot(int _id, string _stellenName, string _beschreibung, string _institut, int _anbieterID,
                              Date _startAnstellung, Date _endeAnstellung, Date _bewerbungsFrist, int _monatsStunden, int _anzahlOffeneStellen,
                              string _ort, string _vorraussetzungen)
        {
            this.id = _id;
            this.stellenName = _stellenName;
            this.ort = _ort;
            this.beschreibung = _beschreibung;
            this.vorraussetzungen = _vorraussetzungen;
            this.monatsStunden = _monatsStunden;
            this.anzahlOffeneStellen = _anzahlOffeneStellen;
            this.institut = _institut;
            //this.anbieterID = _anbieterID;
            this.startAnstellung = _startAnstellung;
            this.endeAnstellung = _endeAnstellung;
            this.bewerbungsFrist = _bewerbungsFrist;
        }

        public Stellenangebot()
        {
            // TODO: Complete member initialization
            this.startAnstellung = new Date();
            this.endeAnstellung = new Date();
            this.bewerbungsFrist = new Date();
        }


        public int id { get; set; }

        //---------------Grunddaten Stellenangebot--------------------------------------

        [Required]
        [StringLength(50)]
        public string stellenName { get; set; }

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