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
                int[] userData = getUserDaten();
                stelle.anbieterID = userData[0];
                if(StelleHinzufügen(stelle))
                {
                    return RedirectToAction("Index","User");
                }
            }

            return View();
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
            int[] userData = getUserDaten();


            string query = "INSERT INTO " +
                                "Stellenangebote " +
                                    "(" +
                                        "stellenName, " +
                                        "beschreibung, " +
                                        "institut, " +
                                        "anbieterID, " +
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
                                        "" + stelle.anbieterID + ",' " +
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
            int[] userData = getUserDaten();
            SqlDataReader reader = DB.auslesen("Select id, stellenName, beschreibung, institut, anbieterID, startAnstellung, endeAnstellung, bewerbungsFrist, monatsStunden, anzahlOffeneStellen, ort, vorraussetzungen " +
                                                "from Stellenangebote where anbieterID = " + userData[0] + "");//HIER GEHTS WEITER
            LinkedList<Stellenangebot> liste = new LinkedList<Stellenangebot>();
            string DateFormat = "dd-MM-yyyy";

            while (reader.Read())
            {
                liste.AddLast(new Stellenangebot(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), Convert.ToInt32(reader.GetValue(4)), new Date(reader.GetDateTime(5).ToString(DateFormat)), 
                                             new Date(reader.GetDateTime(6).ToString(DateFormat)), new Date(reader.GetDateTime(7).ToString(DateFormat)), Convert.ToInt32(reader.GetValue(8)), Convert.ToInt32(reader.GetValue(9)), reader.GetValue(10).ToString(), reader.GetValue(11).ToString())); 
            }
            StellenangebotUebersicht angebote = new StellenangebotUebersicht(liste);
            reader.Close();
            return PartialView(angebote);
        }

        /// <summary>
        /// Läd ein ausgewähltes Stellenangebot mithilfe einer id
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetStelleAngebot(int id, string view)
        {
            Stellenangebot stelle = new Stellenangebot();

            string query = "SELECT stellenName, beschreibung, institut, anbieterID, startAnstellung, endeAnstellung, bewerbungsFrist, monatsStunden, anzahlOffeneStellen, ort, vorraussetzungen FROM Stellenangebote WHERE id=" + id + "";
            SqlDataReader reader = DB.auslesen(query);
            if (reader.HasRows)
            {
                int[] userdata = getUserDaten();
                string dateformat = "dd-MM-yyyy";
                reader.Read();
                stelle.stellenName = reader.GetValue(0).ToString();
                stelle.beschreibung = reader.GetValue(1).ToString();
                stelle.institut = reader.GetValue(2).ToString();
                stelle.anbieterID = Convert.ToInt32(reader.GetValue(3));
                stelle.startAnstellung = new Date(reader.GetDateTime(4).ToString(dateformat));
                stelle.endeAnstellung = new Date(reader.GetDateTime(5).ToString(dateformat));
                stelle.bewerbungsFrist = new Date(reader.GetDateTime(6).ToString(dateformat));
                stelle.monatsStunden = Convert.ToInt32(reader.GetValue(7));
                stelle.anzahlOffeneStellen = Convert.ToInt32(reader.GetValue(8));
                stelle.ort = reader.GetValue(9).ToString();
                stelle.vorraussetzungen = reader.GetValue(10).ToString();
                stelle.id = id;
                reader.Close();
                
                ViewData.Add("Title", "Stellenangebot bearbeiten");
                ViewData.Add("Methode", "StelleAktualisieren");
                if (view == "anzeigen")
                {
                    return View("StellenAngebot", stelle);
                }
                else
                {
                    return View("StellenangebotBearbeiten", stelle);
                }
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
        public ActionResult StelleAktualisieren(Stellenangebot stelle)
        {
            if (ModelState.IsValid)
            {
                int[] userData = getUserDaten();
                string startAnstellung = stelle.startAnstellung.getDate();
                string endeAnstellung = stelle.endeAnstellung.getDate();
                string bewerbungsFrist = stelle.bewerbungsFrist.getDate();

                string query = "UPDATE Stellenangebote SET " +
                                    "stellenName='" + stelle.stellenName + "', " +
                                    "beschreibung='" + stelle.beschreibung + "', " +
                                    "institut='" + stelle.institut + "', " +
                                    "anbieterID=" + userData[0] + ", " +
                                    "startAnstellung='" + startAnstellung + "', " +
                                    "endeAnstellung='" + endeAnstellung + "', " +
                                    "bewerbungsFrist='" + bewerbungsFrist + "', " +
                                    "monatsStunden=" + stelle.monatsStunden + ", " +
                                    "anzahlOffeneStellen=" + stelle.anzahlOffeneStellen + ", " +
                                    "ort='" + stelle.ort + "', " +
                                    "vorraussetzungen='" + stelle.vorraussetzungen + "' " +
                                "WHERE id=" + stelle.id + "";

                DB.aendern(query);

                return View("Stellenangebot", stelle);
            }
            return View("StellenangebotBearbeiten",stelle);
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
