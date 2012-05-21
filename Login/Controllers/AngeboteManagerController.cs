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


        

        /// <summary>
        /// Läd ein ausgewähltes Stellenangebot mithilfe einer id, die View Variable gibt an ob ein Stellenangebot angezeigt wird oder bearbeitet wird
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetStelleAngebot(string id, string view)
        {
            Stellenangebot aktStelle = DB.stellenangebotLesen(Convert.ToInt32(id));
            if (Request.IsAuthenticated)
                {
                    Anbieter anbieter = DB.anbieterAuslesen(HttpContext.User.Identity.Name);
                    Anbieter stellvertreter = DB.anbieterAuslesenID(anbieter.stellvertreterID);
                    ViewBag.Anbieter = false;
                    ViewBag.Stellvertreter = false;
                    if (stellvertreter.email == HttpContext.User.Identity.Name)
                    {
                        ViewBag.Stellvertreter = true;
                    }
                    if (anbieter.id == aktStelle.anbieterID)
                    {
                        ViewBag.Anbieter = true;
                    }
                }
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

        public PartialViewResult _Filter()
        {
            ViewData["Institute"] = new SelectList(DB.institutListeLesen());
            ViewData["MonatsStunden"] = new SelectList(initMonatsStundenDropDown());

            return PartialView("_Filter");

        }

        /// <summary>
        /// Übergibt der Stellenangebote Übersicht alle Stellenangebote
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _StellenAngeboteÜbersicht(Login.Models.Filter filter)
        {
            if (filter.institut == null && filter.monatsStunden == null && filter.Name == null)
            {
                StellenangebotUebersicht angebote = new StellenangebotUebersicht(DB.stellenangeboteUebersichtLesen());
 
                return PartialView("_Stellenangebote", angebote);
            }
            if (filter.institut == "Institute" && filter.monatsStunden == "Monats Stunden" && filter.Name == null)
            {
                StellenangebotUebersicht angebote = new StellenangebotUebersicht(DB.stellenangeboteUebersichtLesen());
                return PartialView("_Stellenangebote", angebote);
            }
            else
            {
                StellenangebotUebersicht filterAngebote = new StellenangebotUebersicht(DB.stellenangeboteUebersichtFiltern(filter));
                return PartialView("_Stellenangebote", filterAngebote);
            }
            
        }

        private LinkedList<string> initMonatsStundenDropDown()
        {
            LinkedList<string> monatsStunden = new LinkedList<string>();
            monatsStunden.AddFirst("Monats Stunden");
            monatsStunden.AddLast(" 0 - 10");
            monatsStunden.AddLast("10 - 20");
            monatsStunden.AddLast("20 - 30");
            monatsStunden.AddLast("30 - 70");

            return monatsStunden;
        }
    }
}
