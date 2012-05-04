using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Models;
using System.Web.Security;
using System.Data.SqlClient;

namespace Login.Controllers
{
    /// <summary>
    /// Der User Controller verwaltet die Registrierung, den Login und die Kontodatenänderungen
    /// </summary>
    public class UserController : Controller
    {
        Benutzer user;
        DBManager DB = DBManager.getInstanz();



        /// <summary>
        /// Ruft die Hauptseite mit login Bereich auf
        /// </summary>
        /// <returns>Index.cshtml</returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// zeigt das Registrierungsformular an
        /// </summary>
        /// <returns>Register.cshtml</returns>
        public ActionResult Register()
        {
            var model = new Benutzer();
            return View(model);
        }

        /// <summary>
        /// fügt einen Benutzer der Datenbank hinzu und startet eine Session.
        /// öffnet die Hauptseite
        /// </summary>
        /// <param name="model">Register model</param>
        /// <returns>Index.cshtml</returns>
        [HttpPost]
        public ActionResult Register(Benutzer model)
        {
            string passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(model.passwort, "SHA1");
            DB.aendern("INSERT INTO " +
                            "Benutzer " +
                                "( vorname, nachname, email, studiengang, fachsemester, strasse, hausnummer, wohnort, plz, passwort, rechte, freischaltung, matrikelnummer, institut, stellvertreterID) " +
                            "VALUES " +
                                "(" +
                                    "'" + model.vorname + "', '" + model.nachname + "', '" + model.email + "', '" + model.studiengang + "', " + model.fachsemester + ", '" + model.strasse + "', '" + model.hausnummer + "', '" + model.wohnort + "', " +
                                    model.plz + ", '" + passwort + "', 0, 1, " + model.matrikelnummer + ", '" + model.institut + "', 12)");
            FormsAuthentication.SetAuthCookie(user.email, false); 
            return RedirectToAction("Index");

        }


        /// <summary>
        /// ruft die Kontodaten des eingeloggten Benutzers ab und gibt sie auf der
        /// Kontoseite zurück
        /// </summary>
        /// <returns>Konto.cshtml</returns>
        [Authorize]
        public ActionResult Konto()
        {
            this.user = new Benutzer();
            user.email = HttpContext.User.Identity.Name;

            string query = "SELECT vorname, nachname, strasse, hausnummer, plz, wohnort, matrikelnummer, studiengang, fachsemester FROM Benutzer WHERE email='" + user.email + "'";
            SqlDataReader reader = DB.auslesen(query);
            reader.Read();
            user.vorname = reader.GetValue(0).ToString();
            user.nachname = reader.GetValue(1).ToString();
            user.strasse = reader.GetValue(2).ToString();
            user.hausnummer = reader.GetValue(3).ToString();
            user.plz = reader.GetInt32(4);
            user.wohnort = reader.GetValue(5).ToString();
            user.matrikelnummer = reader.GetInt32(6);
            user.studiengang = reader.GetValue(7).ToString();
            user.fachsemester = reader.GetValue(8).ToString();

            reader.Close();

            return View(user);
        }


        /// <summary>
        /// zeigt das Kontoformular an auf der die Benutzerdaten verändert werden können
        /// </summary>
        /// <returns>KontoBearbeiten.cshtml</returns>
        [Authorize]
        public ActionResult KontoBearbeiten()
        {
            this.user = new Benutzer();
            user.email = HttpContext.User.Identity.Name;

            string query = "SELECT vorname, nachname, strasse, hausnummer, plz, wohnort, matrikelnummer, studiengang, fachsemester FROM Benutzer WHERE email='" + user.email + "'";
            SqlDataReader reader = DB.auslesen(query);
            reader.Read();
            user.vorname = reader.GetValue(0).ToString();
            user.nachname = reader.GetValue(1).ToString();
            user.strasse = reader.GetValue(2).ToString();
            user.hausnummer = reader.GetValue(3).ToString();
            user.plz = reader.GetInt32(4);
            user.wohnort = reader.GetValue(5).ToString();
            user.matrikelnummer = reader.GetInt32(6);
            user.studiengang = reader.GetValue(7).ToString();
            user.fachsemester = reader.GetValue(8).ToString();

            reader.Close();

            return View(user);
        }

        /// <summary>
        /// Übernimmt die vom Benutzer in die KontoBearbeiten Seite eingetragenen Änderungen
        /// in die Datenbank und leitet den Benutzer auf die Konto Seite weiter
        /// </summary>
        /// <param name="user">Benutzer model</param>
        /// <returns>Konto.cshtml</returns>
        [HttpPost]
        [Authorize]
        public ActionResult KontoBearbeiten(Benutzer user)
        {
            user.email = HttpContext.User.Identity.Name;

            string query = "UPDATE Benutzer SET " +
                                "vorname='" + user.vorname + "', " +
                                "nachname='" + user.nachname + "', " +
                                "strasse='" + user.strasse + "', " +
                                "hausnummer='" + user.hausnummer + "', " +
                                "plz='" + user.plz + "', " +
                                "wohnort='" + user.wohnort + "', " +
                                "matrikelnummer='" + user.matrikelnummer + "', " +
                                "studiengang='" + user.studiengang + "', " +
                                "fachsemester='" + user.fachsemester + "' " +
                            "WHERE email='" + user.email + "'";

            DB.aendern(query);

            return RedirectToAction("Konto");
        }

        /// <summary>
        /// Gleicht die vom Benutzer in das Loginfeld eingegebenen Daten mit der 
        /// Datenbank ab, und setzt das AuthCookie falls Passwort und Email richtig sind.
        /// </summary>
        /// <param name="user">Login model</param>
        /// <returns>Index.cshtml</returns>
        [HttpPost]
        public ActionResult Login(Login.Models.Login user)
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Passwort, "SHA1");

            

            if (ModelState.IsValid) //Model Valedierung ist korrekt (Email Format + Passwort)
            {
                string query = "SELECT passwort FROM Benutzer WHERE email='" + user.Email + "'";
                SqlDataReader reader = DB.auslesen(query);
                reader.Read();
                string pw = reader.GetValue(0).ToString();

                if (password == pw)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false); //Auth-Cookie wird gesetzt, ab jetzt ist man Eingeloggt: False bedeutet: Wenn der Browser geschlossen wird so existiert das cookie auch nicht mehr

                    reader.Close();
                    return RedirectToAction("index", "User");
                }
                else
                { // falsches passwort

                }
                reader.Close();
            }
            else
            {
                ModelState.AddModelError("", "Falsche eingabe");
            }
            return View("index");
        }


        /// <summary>
        /// Meldet den Benutzer ab indem das AuthCookie gelöscht wird
        /// </summary>
        /// <returns>Index.cshtml</returns>
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();//Auth-Cookie wird gelöscht
            return RedirectToAction("Index", "User");
        }


        /// <summary>
        /// Speichert den Benutzer in die Datenbank
        /// </summary>
        /// <param name="user">Register model</param>
        /// <returns>Boolean erfolgreich</returns>
        private bool benutzerSpeichern(Benutzer user)
        {
            string query = "INSERT INTO " +
                                "Benutzer " +
                                    "(" +
                                        "vorname, " +
                                        "nachname, " +
                                        "email, " +
                                        "studiengang, " +
                                        "fachsemester, " +
                                        "strasse, " +
                                        "hausnummer, " +
                                        "wohnort, " +
                                        "plz, " +
                                        "passwort, " +
                                        "rechte, " +
                                        "freischaltung, " +
                                        "matrikelnummer, " +
                                        "institut, " +
                                        "stellvertreterID" +
                                    ") " +
                                "VALUES " +
                                    "(" +
                                        "'" + user.vorname + "', " +
                                        "'" + user.nachname + "', " +
                                        "'" + user.email + "', " +
                                        "'" + user.studiengang + "', " +
                                        user.fachsemester + ", " +
                                        "'" + user.strasse + "', " +
                                        "'" + user.hausnummer + "', " +
                                        "'" + user.wohnort + "', " +
                                        user.plz + ", " +
                                        "'" + user.passwort + "', " +
                                        user.rechte + ", " +
                                        user.freischaltung + ", " +
                                        user.matrikelnummer + ", " +
                                        "'" + user.institut + "', " +
                                        user.stellvertreterID +
                                    ")";
            
            DB.aendern(query);
            return true;
        }

        //TODO
        private bool GetUserByEmail(string email)
        {
            return true;
        }
    }
}
