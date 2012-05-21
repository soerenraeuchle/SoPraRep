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
    /// 
    [StartVorEnde(ErrorMessage = "Begin der Anstellung muss vor dem Ende sein")]
    [BewerbungsFristVorStart(ErrorMessage = "Die Bewerbungsfrist muss vor dem Begin der Anstellung enden")]
    public class Stellenangebot
    {

        public Stellenangebot(int _id, string _stellenName, string _beschreibung, string _institut, int _anbieterID,
                              DateTime _startAnstellung, DateTime _endeAnstellung, DateTime _bewerbungsFrist, int _monatsStunden, int _anzahlOffeneStellen,
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
            this.anbieterID = _anbieterID;
            this.startAnstellung = _startAnstellung;
            this.endeAnstellung = _endeAnstellung;
            this.bewerbungsFrist = _bewerbungsFrist;
        }

        public Stellenangebot()
        {
            // TODO: Complete member initialization
            this.startAnstellung = DateTime.Today;
            this.endeAnstellung = DateTime.Today;
            this.bewerbungsFrist = DateTime.Today;
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

        [Integer]
        public int anbieterID { get; set; }

        //---------------Zeitangaben--------------------------------------

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public  DateTime startAnstellung { get; set; }
        

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime endeAnstellung { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime bewerbungsFrist { get; set; }



        [AttributeUsage(AttributeTargets.Class)]
        public class StartVorEnde : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var model = (Stellenangebot)value;
                return model.startAnstellung < model.endeAnstellung;
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class BewerbungsFristVorStart : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var model = (Stellenangebot)value;
                return model.bewerbungsFrist < model.startAnstellung;
            }
        }



        

        
    }
}