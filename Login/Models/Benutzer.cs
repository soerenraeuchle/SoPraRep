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
       
        [Integer]
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

<<<<<<< HEAD
        public bool freischaltung { get; set; }

        [StringLength(50)]
=======
        
        public bool freischaltung { get; set; }

        
    }

    public class Anbieter : Benutzer
    {
        //---------------Veranstalter relevant--------------------------------------
        public Anbieter()
        {
            this.rechte = 1;
            this.stellvertreterID = -1;
        }

>>>>>>> origin/michi
        [Required]
        [StringLength(50)]
        public string institut { get; set; }

        [Integer]
        public int stellvertreterID { get; set; }
<<<<<<< HEAD

=======


    }

    public class Bewerber : Benutzer
    {
        public Bewerber()
        {
            this.rechte = 0;
        }
        [Required]
        [StringLength(50)]
        public string strasse { get; set; }

        [Required]
>>>>>>> origin/michi
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
<<<<<<< HEAD
        public string fachsemester { get; set; }
=======
        [Integer]
        public int matrikelnummer { get; set; }

        [Required]
        [StringLength(50)]
        public string studiengang { get; set; }

        [Required]
        [Integer]
        public int fachsemester { get; set; }
>>>>>>> origin/michi
    }
}