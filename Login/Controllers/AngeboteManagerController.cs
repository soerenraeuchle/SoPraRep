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
        [HttpGet]
        public ActionResult NeuesStellenAngebot()
        {
            Stellenangebot stelle = new Stellenangebot();
            ViewData.Add("Title", "Neues Stellenangebot erstellen");
            ViewData.Add("Methode", "NeuesStellenAngebot");
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
                if(StelleHinzufügen(stelle))
                {
                    RedirectToAction("index","User");
                }
            }

            return View("neuesStellenAngebot");
        }


        /// <summary>
        /// Die Methode StelleHinzufügen erzeugt das DB Statement um eine neue Stelle in die Datenbank zu speichern und schickt die query an den DBManager
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [Authorize]
        private bool StelleHinzufügen(Stellenangebot stelle)
        {
            string startAnstellung = stelle.startAnstellung.getDate();
            string endeAnstellung = stelle.endeAnstellung.getDate();
            string bewerbungsFrist = stelle.bewerbungsFrist.getDate();

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
                                        "'" + stelle.stellenName + "', " +
                                        "'" + stelle.beschreibung + "', " +
                                        "'" + stelle.institut + "', " +
                                        "'" + stelle.anbieter + "',' " +
                                        startAnstellung + "', " +
                                        "'" + endeAnstellung + "', " +
                                        "'" + bewerbungsFrist + "', " +
                                        "'" + stelle.monatsStunden + "', " +
                                        stelle.anzahlOffeneStellen + ", " +
                                        "'" + stelle.ort + "', '" +
                                        stelle.vorraussetzungen + "')";

            try
            {
                DB.aendern(query);
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
        }

        public PartialViewResult _StellenAngebotSteuerung()
        {
            String [] userData = getUserDaten();
            SqlDataReader reader = DB.auslesen("Select id, stellenName, beschreibung, institut, anbieter, startAnstellung, endeAnstellung, bewerbungsFrist, monatsStunden, anzahlOffenerStellen, ort, vorraussetzungen" +
                                                "from Stellenangebote where anbieter = '" + userData[0] + "'");//HIER GEHTS WEITER
            List<Stellenangebot> liste = new List<Stellenangebot>();

            while (reader.Read())
            {
                liste.Add(new Stellenangebot(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(5), 
                                             new Date(reader.GetValue(6)), reader.GetValue(7), Convert.ToInt32(reader.GetValue(8)), Convert.ToInt32(reader.GetValue(9)), reader.GetValue(10).ToString(), reader.GetValue(11).ToString()); 
            }
            StellenangebotUebersicht angebote = new StellenangebotUebersicht();
            return PartialView();
        }

        /// <summary>
        /// Läd ein ausgewähltes Stellenangebot mithilfe einer id
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult StelleBearbeiten(int id)
        {
            Stellenangebot stelle = new Stellenangebot();

            //StellenID muss hier definiert werden!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            int id = 1;

            string query = "SELECT stellenName, beschreibung, institut, anbieter, startAnstellung, endeAnstellung, bewerbungsFrist, monatsStunden, anzahlOffeneStellen, ort, vorraussetzungen FROM Stellenangebote WHERE id=" + id + "";
            SqlDataReader reader = DB.auslesen(query);
            if (reader.HasRows)
            {

                reader.Read();
                string DateFormat = "dd-MM-yyyy";
                stelle.stellenName = reader.GetValue(0).ToString();
                stelle.beschreibung = reader.GetValue(1).ToString();
                stelle.institut = reader.GetValue(2).ToString();
                stelle.anbieter = reader.GetValue(3).ToString();
                stelle.startAnstellung = new Date(reader.GetDateTime(4).ToString(DateFormat));
                stelle.endeAnstellung = new Date(reader.GetDateTime(5).ToString(DateFormat));
                stelle.bewerbungsFrist = new Date(reader.GetDateTime(6).ToString(DateFormat));
                stelle.monatsStunden = Convert.ToInt32(reader.GetValue(7));
                stelle.anzahlOffeneStellen = Convert.ToInt32(reader.GetValue(8));
                stelle.ort = reader.GetValue(9).ToString();
                stelle.vorraussetzungen = reader.GetValue(10).ToString();

                reader.Close();
                return View("StellenAngebot", stelle);
            }
            reader.Close();

            return View();
        }


        /// <summary>
        /// Aktualisiert ein Stellenangebot in der Datenbank
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult StelleBearbeiten(Stellenangebot stelle)
        {
            if (ModelState.IsValid)
            {

                int id = stelle.id;

                string startAnstellung = stelle.startAnstellung.getDate();
                string endeAnstellung = stelle.endeAnstellung.getDate();
                string bewerbungsFrist = stelle.bewerbungsFrist.getDate();

                string query = "UPDATE Stellenangebote SET " +
                                    "stellenName='" + stelle.stellenName + "', " +
                                    "beschreibung='" + stelle.beschreibung + "', " +
                                    "institut='" + stelle.institut + "', " +
                                    "anbieter='" + stelle.anbieter + "', " +
                                    "startAnstellung='" + startAnstellung + "', " +
                                    "endeAnstellung='" + endeAnstellung + "', " +
                                    "bewerbungsFrist='" + bewerbungsFrist + "', " +
                                    "monatsStunden=" + stelle.monatsStunden + ", " +
                                    "anzahlOffeneStellen=" + stelle.anzahlOffeneStellen + ", " +
                                    "ort='" + stelle.ort + "' " +
                                    "vorraussetzungen='" + stelle.vorraussetzungen + "' " +
                                "WHERE id=" + stelle.id + "";

                DB.aendern(query);
                ViewData.Add("Title", "Stellenangebot bearbeiten");
                ViewData.Add("Methode", "StelleBearbeiten");

                return RedirectToAction("StellenAngebot");
            }
            return View();
        }


        /// <summary>
        /// Löscht eine bestehende StellenAnzeige aus der Datenbank
        /// </summary>
        /// <param name="stelle"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult StelleLöschen(Stellenangebot stelle)
        {
            int id = stelle.id;
            string query = "DELETE FROM Stellenangebote" +
                            "WHERE id ="+id;

            DB.aendern(query);

            return View();
        }

        /// <summary>
        /// liest die hinterlegten Benutzerdaten aus dem AuthCookie
        /// </summary>
        /// <returns>string[] userDaten</returns>
        public string[] getUserDaten()
        {
            FormsIdentity ident = User.Identity as FormsIdentity;
            if (ident != null)
            {
                FormsAuthenticationTicket ticket = ident.Ticket;
                string userDataString = ticket.UserData;

                // string nach | teilen
                string[] userDataPieces = userDataString.Split('|');
                return userDataPieces;
            }
            else
            {
                return null;
            }
        }
    }
}
