using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Models;
using System.Data.SqlClient;
using System.Web.Security;

namespace Login.Controllers
{
    public class AngeboteManagerController : Controller
    {
        //
        // GET: /AngeboteManager/
        DBManager DB = DBManager.getInstanz();

        [Authorize(Roles = "Anbieter")]
        public ActionResult NeuesStellenAngebot()
        {
            Stellenangebot stelle = new Stellenangebot();
            ViewBag.Title = "Stellenangebot erstellen";
            ViewBag.Methode = "NeueStelleSpeichern";
            return View("StellenangebotBearbeiten",stelle);
        }


        /// <summary>
        /// Die Methode "neueStelleSpeichern" speichert ein neu angelegtes Stellenangebot
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Anbieter")]
        public ActionResult NeueStelleSpeichern(Stellenangebot stelle)
        {
            if(ModelState.IsValid)
            {
                string email = HttpContext.User.Identity.Name;
                Anbieter benutzer = DB.anbieterAuslesen(email);
                stelle.anbieterID = benutzer.id;
                if(DB.stellenangebotHinzufügen(stelle))
                {
                    return RedirectToAction("Index","User");
                }
            }

            return View();
        }




        /// <summary>
        /// Läd alle eigenen Stellenangebote in eine Liste und fügt sie der Partiellen View _StellenangeboteÜbersicht hinzu
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Anbieter")]
        public PartialViewResult _StellenAngebotSteuerung()
        {
            string email = HttpContext.User.Identity.Name;
            Anbieter benutzer = DB.anbieterAuslesen(email);


            StellenangebotUebersicht angebote = new StellenangebotUebersicht(DB.stellenangebotUebersichtLesen(benutzer.id));
            return PartialView(angebote);

        }

        public PartialViewResult _StellenAngeboteÜbersicht()
        {
            StellenangebotUebersicht angebote = new StellenangebotUebersicht(DB.stellenangeboteUebersichtLesen());
            return PartialView("_Stellenangebote",angebote);

        }

        /// <summary>
        /// Läd ein ausgewähltes Stellenangebot mithilfe einer id, die View Variable gibt an ob ein Stellenangebot angezeigt wird oder bearbeitet wird
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetStelleAngebot(Stellenangebot stelle, string view)
        {
            Stellenangebot aktStelle = DB.stellenangebotLesen(stelle.id);
            ModelState.Clear();
            if (view == "anzeigen")
            {
                ViewBag.Title = "Stellenangebot erstellen";
                ViewBag.Methode = "NeueStelleSpeichern";
                return View("StellenAngebot", aktStelle);
            }
            else
            {
                
                ViewBag.Title = "Stellenangebot bearbeiten";
                ViewBag.Methode = "StelleAktualisieren";
                return View("StellenangebotBearbeiten", aktStelle);
            }
            
        }


        /// <summary>
        /// Aktualisiert ein Stellenangebot in der Datenbank
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Anbieter")]
        public ActionResult StelleAktualisieren(Stellenangebot stelle)
        {
            if (ModelState.IsValid)
            {
                string email = HttpContext.User.Identity.Name;
                Anbieter benutzer = DB.anbieterAuslesen(email);
                stelle.anbieterID = benutzer.id;
                DB.stellenangebotAktualisieren(stelle);

                return View("StellenAngebot", stelle);
            }
            return View("StellenangebotBearbeiten",stelle);
        }


        /// <summary>
        /// Löscht eine bestehende StellenAnzeige aus der Datenbank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Anbieter")]
        public ActionResult StelleLöschen(Stellenangebot stelle)
        {
            DB.stellenangebotLoeschen(stelle);
            return RedirectToAction("Index", "User");
        }
    }
}
