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

        [Authorize]
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
        [Authorize]
        public ActionResult NeueStelleSpeichern(Stellenangebot stelle)
        {
            if(ModelState.IsValid)
            {
                int[] userData = getUserDaten();
                stelle.anbieterID = userData[0];
                if(DB.stellenangebotHinzufügen(stelle))
                {
                    return RedirectToAction("Index","User");
                }
            }

            return View();
        }





        public PartialViewResult _StellenAngebotSteuerung()
        {
            int[] userData = getUserDaten();

            StellenangebotUebersicht angebote = new StellenangebotUebersicht(DB.stellenangebotUebersichtLesen(userData[0]));
            return PartialView(angebote);
        }

        /// <summary>
        /// Läd ein ausgewähltes Stellenangebot mithilfe einer id
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetStelleAngebot(Stellenangebot stelle, string view)
        {
            Stellenangebot aktStelle = DB.stellenangebotLesen(stelle.id);

            ViewBag.Title = "Stellenangebot erstellen";
            ViewBag.Methode = "NeueStelleSpeichern";
            

            if (view == "anzeigen")
                return View("StellenAngebot", aktStelle);
            return View("StellenangebotBearbeiten", aktStelle);
            
        }


        /// <summary>
        /// Aktualisiert ein Stellenangebot in der Datenbank
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult StelleAktualisieren(Stellenangebot stelle)
        {
            if (ModelState.IsValid)
            {
                int[] userData = getUserDaten();
                stelle.anbieterID = userData[0];
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
        [Authorize]
        public ActionResult StelleLöschen(Stellenangebot stelle)
        {
            DB.stellenangebotLoeschen(stelle);
            return RedirectToAction("Index", "User");
        }
    
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


    }
}
