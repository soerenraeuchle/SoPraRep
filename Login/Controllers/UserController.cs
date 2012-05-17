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
<<<<<<< HEAD

=======
        
>>>>>>> origin/michi
        /// <summary>
        /// Ruft die Hauptseite mit login Bereich auf
        /// </summary>
        /// <returns>Index.cshtml</returns>
        public ActionResult Index()
        {
            int[] data = getUserDaten();
            if (data != null)
            {
                ViewData.Add("Rolle", data[1]);
            }
            else
            {
                ViewData.Add("Rolle", 12);
            }
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
        /// Fügt einen Bewerber der Datenbank hinzu und startet eine Session.
        /// öffnet die Hauptseite
        /// </summary>
        /// <param name="model">Register model</param>
        /// <returns>Index.cshtml</returns>
        public ActionResult RegisterBewerber(Bewerber model)
        {
<<<<<<< HEAD
            string passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(model.passwort, "SHA1");
            DB.aendern("INSERT INTO " +
                            "Benutzer " +
                                "( vorname, nachname, email, studiengang, fachsemester, strasse, hausnummer, wohnort, plz, passwort, rechte, freischaltung, matrikelnummer, institut, stellvertreterID) " +
                            "VALUES " +
                                "(" +
                                    "'" + model.vorname + "', '" + model.nachname + "', '" + model.email + "', '" + model.studiengang + "', " + model.fachsemester + ", '" + model.strasse + "', '" + model.hausnummer + "', '" + model.wohnort + "', " +
                                    model.plz + ", '" + passwort + "', 0, 1, " + model.matrikelnummer + ", '" + model.institut + "', 12)");

=======
            if (ModelState.IsValid)
            {
                if (model.rechte == 0) //Registrierung als Admin nicht zulassen.
                {
                    string passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(model.passwort, "SHA1");
                    if (model.rechte == 0)
                    {
                        if (DB.bewerberSpeichern(model))
                        {
                            FormsAuthentication.SetAuthCookie(model.email, false);
                        }
                    }
                    
                }
            }
           
>>>>>>> origin/michi
            return RedirectToAction("Index");
        }


        public ActionResult RegisterAnbieter(Anbieter model)
        {
            if (ModelState.IsValid)
            {
                if (model.rechte != 3)
                {
                    string passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(model.passwort, "SHA1");

                    if (DB.anbieterSpeichern(model))
                    {
                        FormsAuthentication.SetAuthCookie(model.email, false);
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
            if (rechte == 0)
            {
                Bewerber bewerber = new Bewerber();
                return PartialView("_RegisterBewerber", bewerber);
            }
            else
            {
                Anbieter anbieter = new Anbieter();
                return PartialView("_RegisterAnbieter", anbieter);
            }
        }


        /// <summary>
        /// Überprüft ob die Email schon in der Datenbank vorhanden ist.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Ein Json Objekt</returns>
        public JsonResult EmailVorhanden(string email)
        {

            if (DB.emailVorhanden(email))
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
<<<<<<< HEAD
            Benutzer user = new Benutzer();
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
=======
            string email = HttpContext.User.Identity.Name;
            //BewerberKonto
            if (Roles.GetRolesForUser(email)[0].Equals("Bewerber"))
            {
                Bewerber benutzer = DB.bewerberAuslesen(email);
                if (ModelState.IsValid)
                {
                    return View("KontoBewerber", benutzer);
                }
            }
            //AnbieterKonto
            if (Roles.GetRolesForUser(email)[0].Equals("Anbieter"))
            {
                Anbieter benutzer = DB.anbieterAuslesen(email);
                if (ModelState.IsValid)
                {
                    return View("KontoAnbieter", benutzer);
                }
            }
            return View("Index");
>>>>>>> origin/michi
        }


        [Authorize]
        public ActionResult KontoBearbeiten()
        {
            string email = HttpContext.User.Identity.Name;
            
            //Bewerber 
            if (Roles.GetRolesForUser(email)[0].Equals("Bewerber"))
            {
                return RedirectToAction("KontoBewerberBearbeiten");
            }

            //Anbieter
            if (Roles.GetRolesForUser(email)[0].Equals("Anbieter"))
            {
                return RedirectToAction("KontoAnbieterBearbeiten");
            }

            return View("Fehler");
        }




        /// <summary>
        /// zeigt das Kontoformular an auf der die Benutzerdaten verändert werden können
        /// </summary>
        /// <returns>KontoBearbeiten.cshtml</returns>
        [Authorize(Roles="Bewerber")]
        public ActionResult KontoBewerberBearbeiten()
        {
<<<<<<< HEAD
            Benutzer user = new Benutzer();
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
=======
            string email = HttpContext.User.Identity.Name;
            Bewerber benutzer = DB.bewerberAuslesen(email);
            return View(benutzer);
            
        }
        
        /// <summary>
        /// Übernimmt die vom Benutzer in die KontoBearbeiten Seite eingetragenen Änderungen
        /// in die Datenbank und leitet den Benutzer auf die Konto Seite weiter
        /// </summary>
        /// <param name="user">Benutzer model</param>
        /// <returns>Konto.cshtml</returns>
        
        [HttpPost]
        [Authorize(Roles = "Bewerber")]
        public ActionResult KontoBewerberBearbeiten(Bewerber benutzer)
        {
            //benutzer.email = HttpContext.User.Identity.Name;

            if (ModelState.IsValid)
            {
                DB.bewerberAktualisieren(benutzer);
                return RedirectToAction("Konto");
            }

            return RedirectToAction("Fehler");
            
        }

        [Authorize(Roles = "Anbieter")]
        public ActionResult KontoAnbieterBearbeiten()
        {
            string email = HttpContext.User.Identity.Name;
            Anbieter benutzer = DB.anbieterAuslesen(email);
            return View(benutzer);

>>>>>>> origin/michi
        }

        /// <summary>
        /// Übernimmt die vom Benutzer in die KontoBearbeiten Seite eingetragenen Änderungen
        /// in die Datenbank und leitet den Benutzer auf die Konto Seite weiter
        /// </summary>
        /// <param name="user">Benutzer model</param>
        /// <returns>Konto.cshtml</returns>

        [HttpPost]
        [Authorize(Roles = "Anbieter")]
        public ActionResult KontoAnbieterBearbeiten(Anbieter benutzer)
        {
<<<<<<< HEAD
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
=======

            if (ModelState.IsValid)
            {
                DB.anbieterAktualisieren(benutzer);
                return RedirectToAction("Konto");
            }

            return RedirectToAction("Fehler");

>>>>>>> origin/michi
        }

        /// <summary>
        /// Gleicht die vom Benutzer in das Loginfeld eingegebenen Daten mit der 
        /// Datenbank ab, und setzt das AuthCookie falls Passwort und Email richtig sind.
        /// </summary>
        /// <param name="user">Login model</param>
        /// <returns>Index.cshtml</returns>
        [HttpPost]
        public ActionResult Login(Login.Models.Login login)
        {
            login.Passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Passwort, "SHA1");

            

            if (ModelState.IsValid) //Model Valedierung ist korrekt (Email Format + Passwort)
            {
<<<<<<< HEAD
                string query = "SELECT id, passwort, rechte FROM Benutzer WHERE email='" + user.Email + "'";
                SqlDataReader reader = DB.auslesen(query);
                reader.Read();
                string pw = reader.GetValue(1).ToString();

                if (reader.HasRows)
                {
                    if (password == pw)
                    {
                        string userDataString = reader.GetValue(0).ToString() + "|" + reader.GetValue(2).ToString();
                        FormsAuthentication.SetAuthCookie(user.Email, false);
                        HttpCookie authCookie = FormsAuthentication.GetAuthCookie(user.Email, false);
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userDataString);
                        authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                        Response.Cookies.Add(authCookie);
                        //Auth-Cookie wird gesetzt, ab jetzt ist man Eingeloggt: False bedeutet: Wenn der Browser geschlossen wird so existiert das cookie auch nicht mehr
=======
                Benutzer check = DB.benutzerAuslesen(login.Email);

                //schauen ob Emailadresse vorhanden
                if (String.IsNullOrEmpty(check.email))
                {
                    ModelState.AddModelError("", "Emailadresse existiert nicht");
                }
                else
                {
                    //passwörter vergleichen und benutzer auslesen
                    if (check.passwort.Equals(login.Passwort))
                    {
                        //je nach Rolle auslesen und cookie setzen
                        if (Roles.GetRolesForUser(check.email)[0].Equals("Bewerber"))
                        {
                            Bewerber benutzer = DB.bewerberAuslesen(check.email);
                            FormsAuthentication.SetAuthCookie(benutzer.email, false); //Auth-Cookie wird gesetzt, ab jetzt ist man Eingeloggt: False bedeutet: Wenn der Browser geschlossen wird so existiert das cookie auch nicht mehr
                        }

                        if (Roles.GetRolesForUser(check.email)[0].Equals("Anbieter"))
                        {
                            Anbieter benutzer = DB.anbieterAuslesen(check.email);
                            FormsAuthentication.SetAuthCookie(benutzer.email, false); //Auth-Cookie wird gesetzt, ab jetzt ist man Eingeloggt: False bedeutet: Wenn der Browser geschlossen wird so existiert das cookie auch nicht mehr
                        }
>>>>>>> origin/michi

                        return RedirectToAction("index", "User");
<<<<<<< HEAD
                    }
                }
                else
                { // falsches passwort

                }
                reader.Close();
=======

                    }
                    else
                    {
                        //passwort falsch
                        ModelState.AddModelError("", "Passwort falsch");
                    }
                }
                

>>>>>>> origin/michi
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


<<<<<<< HEAD
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

        /// <summary>
        /// liest die hinterlegten Benutzerdaten aus dem AuthCookie
        /// </summary>
        /// <returns>string[] userDaten</returns>
        public int[] getUserDaten()
        {
            FormsIdentity ident = User.Identity as FormsIdentity;
            if (ident != null)
            {
                FormsAuthenticationTicket ticket = ident.Ticket;
                string userDataString = ticket.UserData;

                // string nach | teilen
                String[] userDataPieces = userDataString.Split('|');
                int[] userData = new int[2];
                userData[0] = Convert.ToInt32(userDataPieces[0]);
                userData[1] = Convert.ToInt32(userDataPieces[1]);

                return userData;
            }
            else
            {
                return null;
            }
        }

        //TODO
        private bool GetUserByEmail(string email)
        {
            return true;
        }
=======
       
>>>>>>> origin/michi
    }
}
