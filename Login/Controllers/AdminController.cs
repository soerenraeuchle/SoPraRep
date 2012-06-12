using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Models;


namespace Login.Controllers
{

    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        DBManager DB = DBManager.getInstanz();

        /// <summary>
        /// Ruft die Übersicht des Admincenters auf. 
        /// </summary>
        /// <returns>Übersicht des Admincenters.</returns>
        public ActionResult Index()
        {

            LinkedList<Benutzer> benutzerList = DB.benutzerAuslesen();
            BenutzerUebersicht uebersicht = new BenutzerUebersicht(benutzerList);
            return View(uebersicht);

        }

        /// <summary>
        /// Legt einen neuen Admin in der Datenbank an.
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminAnlegen()
        {
            Admin admin = new Admin();
            admin.vorname = "Vorname";
            admin.nachname = "Nachname";
            admin.email = "admin@uni-ulm.de";
            admin.passwort = "admin";
            DB.adminAnlegen(admin);
            return RedirectToAction("Index");

        }


        /// <summary>
        /// Löscht einen Benutzer.
        /// </summary>
        /// <param name="benutzerID"></param>
        /// <returns>ID des Benutzers.</returns>
        [HttpPost]
        public ActionResult BenutzerLoeschen(int benutzerID)
        {
            DB.benutzerLoeschen(benutzerID);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult BenutzerFreischalten(int benutzerID)
        {
            DB.benutzerFreischalten(benutzerID);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult BenutzerSperren(int benutzerID)
        {
            DB.benutzerSperren(benutzerID);
            return RedirectToAction("Index");
        }

        public ActionResult StellenangeboteAuslesen()
        {
            StellenangebotUebersicht angebote = new StellenangebotUebersicht(DB.stellenangeboteUebersichtLesen());
            return View(angebote);
        }
            
    }
}
