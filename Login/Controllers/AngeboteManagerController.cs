using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Models;

namespace Login.Controllers
{
    public class AngeboteManagerController : Controller
    {
        //
        // GET: /AngeboteManager/
        DBManager DB = DBManager.getInstanz();

        public ActionResult Filter()
        {
            return View();
        }

        [Authorize]
        public ActionResult neuesStellenAngebot()
        {
            Stellenangebot stelle = new Stellenangebot();
            return View(stelle);
        }

        /// <summary>
        /// Die Methode "neueStelleSpeichern" speichert ein neu angelegtes Stellenangebot
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult neueStelleSpeichern(Stellenangebot stelle)
        {
            if(ModelState.IsValid)
            {
                if(StelleHinzufügen(stelle))
                {
                    RedirectToAction("index","User");
                }
            }

            return View("neuesStellenAngebot");
        }

        private bool StelleHinzufügen(Stellenangebot angebot)
        {
            string startAnstellung = angebot.startAnstellung.getDate();
            string endeAnstellung = angebot.endeAnstellung.getDate();
            string bewerbungsFrist = angebot.bewerbungsFrist.getDate();

            string query = "INSERT INTO " +
                                "Stellenangebote " +
                                    "(" +
                                        "stellenName, " +
                                        "beschreibung, " +
                                        "institut, " +
                                        "anbieter, " +
                                        "startAnstellung, " +
                                        "endeAnstellung, " +
                                        "bewerbungsFrist, " +
                                        "monatsStunden, " +
                                        "anzahlOffeneStellen, " +
                                        "ort, " +
                                        "vorraussetzungen " + ") " +
                                "VALUES " +
                                    "(" +
                                        "'" + angebot.stelllenName + "', " +
                                        "'" + angebot.beschreibung + "', " +
                                        "'" + angebot.institut + "', " +
                                        "'" + angebot.anbieter + "',' " +
                                        startAnstellung + "', " +
                                        "'" + endeAnstellung + "', " +
                                        "'" + bewerbungsFrist + "', " +
                                        "'" + angebot.monatsStunden + "', " +
                                        angebot.anzahlOffeneStellen + ", " +
                                        "'" + angebot.ort + "', '" +
                                        angebot.vorraussetzungen + "')";

            DB.aendern(query);
            return true;
        }

        public ActionResult StelleBearbeiten()
        {
            return View();
        }


    }
}
