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

        private bool StelleHinzufügen(Stellenangebot angebot)
        {
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
                                        "'" + angebot.anbieter + "', " +
                                        angebot.startAnstellung + ", " +
                                        "'" + angebot.endeAnstellung + "', " +
                                        "'" + angebot.bewerbungsFrist + "', " +
                                        "'" + angebot.monatsStunden + "', " +
                                        angebot.anzahlOffeneStellen + ", " +
                                        "'" + angebot.ort + "', " +
                                        angebot.vorraussetzungen + ")";

            DB.aendern(query);
            return true;
        }

        public ActionResult StelleBearbeiten()
        {
            return View();
        }


    }
}
