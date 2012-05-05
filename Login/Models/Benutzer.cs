//test

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    //------------------------------------------------------------------------------------------
    //---------------Benutzer-------------------------------------------------------------------
    public class Benutzer
    {
        [Integer]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string vorname { get; set; }

        [Required]
        [StringLength(50)]
        public string nachname { get; set; }

        [Required]
        [StringLength(50)]
        public string strasse { get; set; }

        [Required]
        [Integer]
        public string hausnummer { get; set; }

        [Required]
        [Integer]
        public int plz { get; set; }

        [Required]
        [StringLength(50)]
        public string wohnort { get; set; }

        //---------------Login relevant--------------------------------------

        [Email]
        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string passwort { get; set; }

        [Required]
        [EqualTo("passwort")]
        [StringLength(50)]
        public string confirmPasswort { get; set; }

        //---------------Rechte Vergabe--------------------------------------

        [Integer]
        public int rechte { get; set; }

        public bool freischaltung { get; set; }

        [StringLength(50)]
        [Required]
        public string institut { get; set; }

        [Integer]
        [Required]
        public int stellvertreterID { get; set; }

        [Integer]
        [Required]
        public int matrikelnummer { get; set; }

        [StringLength(50)]
        [Required]
        public string studiengang { get; set; }

        [Integer]
        [Required]
        public string fachsemester { get; set; }
    }
}