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
        /// Fügt einen Bewerber der Datenbank hinzu und startet eine Session.
        /// öffnet die Hauptseite
        /// </summary>
        /// <param name="model">Register model</param>
        /// <returns>Index.cshtml</returns>
        public ActionResult RegisterBewerber(Bewerber model)
        {
            if (ModelState.IsValid)
            {
                if (model.rechte == 0) //Registrierung als Admin nicht zulassen.
                {
                    string passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(model.passwort, "SHA1");
                    
                    if (DB.bewerberSpeichern(model))
                    {
                        FormsAuthentication.SetAuthCookie(model.email, false);
                    }
                    
                    
                }
            }


            return RedirectToAction("index");
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
        }

        [HttpPost]
        [Authorize(Roles = "Anbieter")]
        public ActionResult KontoAnbieterBearbeiten(Anbieter benutzer)
        {
            if (ModelState.IsValid)
            {
                DB.anbieterAktualisieren(benutzer);
                return RedirectToAction("Konto");
            }
            return RedirectToAction("Fehler");
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
            if (ModelState.IsValid) //Model Valedierung ist korrekt (Email Format + Passwort)
            {
                login.Passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Passwort, "SHA1");
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
                        return RedirectToAction("index", "user");
                    }
                    else
                    {
                        //passwort falsch
                        ModelState.AddModelError("", "Passwort falsch");
                    }
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

    }
}
