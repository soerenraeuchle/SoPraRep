using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Models;
using System.Web.Security;
using System.Data.SqlClient;
using System.Collections;

namespace Login.Controllers
{
    /// <summary>
    /// Der User Controller verwaltet die Registrierung, den Login und die Kontodatenänderungen
    /// </summary>
    public class UserController : Controller
    {
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
        public ActionResult RegisterRolle(Benutzer model)
        {
            if (ModelState.IsValid)
            {
                if (model.rechte != 3) //Registrierung als Admin nicht zulassen.
                {
                    string passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(model.passwort, "SHA1");
                    if (DB.benutzerSpeichern(model))
                    {
                        FormsAuthentication.SetAuthCookie(model.email, false);
                        //return RedirectToAction("Index");
                    }
                }
            }
           
            return RedirectToAction("Index");
        }

        
        /// <summary>
        /// Gibt das zum Wert passende Formular zurück.
        /// </summary>
        /// <param name="rechte">
        /// Wert der die Rechte des Benutzers beschreibt.
        /// </param>
        /// <returns>
        /// Formular
        /// </returns>
        public ActionResult RegisterRolle(int rechte)
        {
            Benutzer model = new Benutzer();
            model.rechte = rechte;
            if (model.rechte == 0)
            {
                return PartialView("_RegisterBewerber", model);
            }
            else
            {
                return PartialView("_RegisterAnbieter", model);
            }
        }


        public JsonResult EmailVorhanden(string email)
        {
            string query = "SELECT id FROM Benutzer WHERE email='" + email + "'";

            ArrayList daten = DB.auslesen(query);

            if (daten.Count>0)
            {
                return Json("Email schon vorhanden", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        
        /// <summary>
        /// ruft die Kontodaten des eingeloggten Benutzers ab und gibt sie auf der
        /// Kontoseite zurück
        /// </summary>
        /// <returns>Konto.cshtml</returns>
        [Authorize]
        public ActionResult Konto()
        {
            //string email = HttpContext.User.Identity.Name;
            Benutzer benutzer = GetSessionBenutzer();
            if (benutzer.rechte == 0)
            {
                return RedirectToAction("KontoBewerber");
            }
            else
            {
                return RedirectToAction("KontoAnbieter");
            }
        }


        [Authorize]
        public ActionResult KontoBewerber()
        {
            Benutzer benutzer = GetSessionBenutzer();
            return View(benutzer);
        }


        [Authorize]
        public ActionResult KontoAnbieter()
        {
            Benutzer benutzer = GetSessionBenutzer();
            return View(benutzer);
        }

        /// <summary>
        /// zeigt das Kontoformular an auf der die Benutzerdaten verändert werden können
        /// </summary>
        /// <returns>KontoBearbeiten.cshtml</returns>
        [Authorize(Roles = "Anbieter")]
        public ActionResult KontoBearbeiten()
        {
            //string email = HttpContext.User.Identity.Name;
            Benutzer benutzer = GetSessionBenutzer();

            return View(benutzer);
        }
        
        /// <summary>
        /// Übernimmt die vom Benutzer in die KontoBearbeiten Seite eingetragenen Änderungen
        /// in die Datenbank und leitet den Benutzer auf die Konto Seite weiter
        /// </summary>
        /// <param name="user">Benutzer model</param>
        /// <returns>Konto.cshtml</returns>
        [HttpPost]
        [Authorize(Roles="Anbieter")]
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
            user.Passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Passwort, "SHA1");

            if (ModelState.IsValid) //Model Valedierung ist korrekt (Email Format + Passwort)
            {
                string query = "SELECT passwort, rechte FROM Benutzer WHERE email='" + user.Email + "'";
                ArrayList daten = DB.auslesen(query);
                if (daten == null)
                {
                    return View("index"); //Fehlerbehandlung
                }
                
                if (daten.Count != 0)
                {
                    ArrayList zeile = (ArrayList)daten[0];

                    string pw = (string)zeile[0];
                    int rechte = (int)zeile[1];

                    if (user.Passwort == pw)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, false); //Auth-Cookie wird gesetzt, ab jetzt ist man Eingeloggt: False bedeutet: Wenn der Browser geschlossen wird so existiert das cookie auch nicht mehr
                        Benutzer benutzer = new Benutzer();
                        benutzer.email = user.Email;
                        benutzer.passwort = user.Passwort;
                        benutzer.rechte = rechte;
                        HttpContext.Session["benutzer"] = benutzer;

                        return RedirectToAction("index", "User");
                    }
                    
                    ModelState.AddModelError("", "Passwort falsch");
                    

                }
                else
                {
                    ModelState.AddModelError("", "Emailadresse existiert nicht");
                }

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
        //private bool benutzerSpeichern(Benutzer user)
        //{
        //    string query;
        //    user.passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(user.passwort, "SHA1");
        //    Object[] values;

        //    if (user.rechte == 0)
        //    {
        //        user.freischaltung = true;
        //        query =     "INSERT INTO " +
        //                        "Benutzer " +
        //                            "(" +
        //                                "vorname, " +
        //                                "nachname, " +

        //                                "email, " +
        //                                "passwort, " +

        //                                "rechte, " +
        //                                "freischaltung, " +

        //                                "studiengang, " +
        //                                "fachsemester, " +

        //                                "strasse, " +
        //                                "hausnummer, " +
        //                                "plz, " +
        //                                "wohnort, " +

        //                                "matrikelnummer " +
        //                            ") " +
        //                        "VALUES " +
        //                            "(" +
        //                                "'" + user.vorname + "', " +
        //                                "'" + user.nachname + "', " +
        //                                "'" + user.email + "', " +
        //                                "'" + user.passwort + "', " +

        //                                user.rechte + ", " +
        //                                "'" + user.freischaltung + "', " +

        //                                "'" + user.studiengang + "', " +
        //                                user.fachsemester + ", " +
        //                                "'" + user.strasse + "', " +
        //                                "'" + user.hausnummer + "', " +
        //                                user.plz + ", " +
        //                                "'" + user.wohnort + "', " +

        //                                user.matrikelnummer + " " +

        //                            ")";
        //        values = new Object[1];
        //    }
        //    else
        //    {
        //        user.freischaltung = false;

        //        query =     "INSERT INTO " +
        //                        "Benutzer " +
        //                            "(" +
        //                                "vorname, " +
        //                                "nachname, " +

        //                                "email, " +
        //                                "passwort, " +

        //                                "rechte, " +
        //                                "freischaltung, " +

        //                                "institut " +
        //                                //"stellvertreterID" + Achtung Komma nach institut gelöscht
        //                            ") " +
        //                        "VALUES " +
        //                            "(" +
        //                                "'" + user.vorname + "', " +
        //                                "'" + user.nachname + "', " +
        //                                "'" + user.email + "', " +
        //                                "'" + user.passwort + "', " +

        //                                user.rechte + ", " +
        //                                "'" + user.freischaltung + "', " +

        //                                "'" + user.institut + "' " +

        //                                //user.stellvertreterID + Achtung Komma nach institut gelöscht

        //                            ")";
                    
        //    }

            
        //    //int affectedRows = DB.aendernPrepared(query, values);
        //    if (affectedRows == -1)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //TODO
        [Authorize]
        private Benutzer GetSessionBenutzer()
        {
            Benutzer user = new Benutzer();
            user = (Benutzer)HttpContext.Session["benutzer"];

            string query;
            if (user.rechte == 0)
            {
                query = "SELECT vorname, nachname, strasse, hausnummer, plz, wohnort, matrikelnummer, studiengang, fachsemester FROM Benutzer WHERE email='" + user.email + "'";
            }
            else
            {
                query = "SELECT vorname, nachname, institut FROM Benutzer WHERE email='" + user.email + "'";
            }
            ArrayList daten = DB.auslesen(query);
            ArrayList zeile = (ArrayList)daten[0];
            user.vorname = (string)zeile[0];
            user.nachname = (string)zeile[1];
            if (user.rechte == 0)
            {
                user.strasse = (string)zeile[2];
                user.hausnummer = (string)zeile[3];
                user.plz = (int)zeile[4];
                user.wohnort = (string)zeile[5];
                user.matrikelnummer = (int)zeile[6];
                user.studiengang = (string)zeile[7];
                user.fachsemester = (int)zeile[8];
            }
            else
            {
                user.institut = (string)zeile[2];
            }
            
            return user;
        }
    }
}
