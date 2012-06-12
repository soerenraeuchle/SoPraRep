//test

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Login.Models
{
    public class Login
    {
        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Passwort { get; set; }
    }

    public class Benutzer
    {
       

        public int id { get; set; }

        //---------------Persönliche Daten--------------------------------------

        [Required]
        [StringLength(50)]
        public string vorname { get; set; }

        [Required]
        [StringLength(50)]
        public string nachname { get; set; }


        //---------------Login relevant--------------------------------------

        [Email]
        [Required]
        [StringLength(50)]
        [Remote("EmailVorhanden", "User")]
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

        
    }

    //TODO<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    public class Admin : Benutzer
    {
        public Admin()
        {
            this.rechte = 3;
            this.freischaltung = true;
        }
    }

    public class Bearbeiter : Benutzer
    {
        public Bearbeiter()
        {
            this.rechte = 2;
            this.freischaltung = false;
        }
    }

    public class Anbieter : Benutzer
    {
        //---------------Veranstalter relevant--------------------------------------
        public Anbieter()
        {
            this.rechte = 1;
            this.stellvertreterID = -1;
        }



        [Required]
        [StringLength(50)]
        public string institut { get; set; }

        [Integer]
        public int stellvertreterID { get; set; }


    }

    public class Bewerber : Benutzer
    {
        public Bewerber()
        {
            this.rechte = 0;
            this.freischaltung = true;
        }


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

        //---------------Studium Daten--------------------------------------
        [Required]
        [Integer]
        public int fachsemester { get; set; }

        [Integer]
        public int matrikelnummer { get; set; }

        [Required]
        [StringLength(50)]
        public string studiengang { get; set; }

    }
}