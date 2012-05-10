using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections;




namespace Login.Models
{
    /// <summary>
    /// Eine Singleton Klasse, die die Verbindung zur Datenbank enthält und SQL Befehle ausführen kann.
    /// </summary>
    public class DBManager
    {
        private DBManager() { }
        private static DBManager instance = null;

        //private static string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\dev\\asp\\sopra\\Login\\Login\\App_Data\\Sopra.mdf;Integrated Security=True;User Instance=True";
        private static string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|Sopra.mdf;Integrated Security=True;User Instance=True";
        private SqlConnection con = new SqlConnection(ConnectionString);

        /// <summary>
        /// Gibt die Instanz der DBManager-Klasse zurück.
        /// </summary>
        /// <returns>
        /// DBManager Instanz
        /// </returns>
        public static DBManager getInstanz()
        {
            if (instance == null)
            {
                instance = new DBManager();
                if (!instance.connect())
                {
                    return null; 
                }
            }
            
            return instance;
        }
            

        // Baut eine Verbindung zur Datenbank auf
        private bool connect()
        {
            try
            {
                con.Open();
                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        private bool disconntect()
        {
            con.Close();
            return true;
        }
        

        /// <summary>
        /// Führt einen SQL Befehl aus, der Zeilen zurückgibt.
        /// </summary>
        /// <param name="query">
        /// SQL Befehl
        /// </param>
        /// <returns>
        /// Gibt ein SqlDataReader Objekt zurück, das die Daten des Querys enthält. Im Fehlerfall wird null zurückgegeben.
        /// </returns>
        public SqlDataReader auslesen(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }


        /// <summary>
        /// Führt einen SQL Befehl aus, der schreibend auf die Datenbank zugreift.
        /// </summary>
        /// <param name="query">
        /// SQL Befehl
        /// </param>
        /// <returns>
        /// Gibt die Anzahl der betroffenen Zeilen zurück.
        /// </returns>
        public int aendern(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return -1;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------
        //-------------------------------------STELLENANGEBOTE PREPARED STATEMENTS--------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------
        public bool stellenangebotAktualisieren(Models.Stellenangebot stelle)
        {
            string startAnstellung = stelle.startAnstellung.getDate();
            string endeAnstellung = stelle.endeAnstellung.getDate();
            string bewerbungsFrist = stelle.bewerbungsFrist.getDate();

            string query = "UPDATE Stellenangebote SET " +
                                    "stellenName= @stellenName, " +
                                    "beschreibung = @beschreibung, " +
                                    "institut = @institut, " +
                                    "anbieterID = @anbieterID, " +
                                    "startAnstellung = @startanstellung, " +
                                    "endeAnstellung = @endeAnstellung, " +
                                    "bewerbungsfrist = @bewerbungsfrist, " +
                                    "monatsStunden = @monatsStunden, " +
                                    "anzahlOffeneStellen = @anzahlOffeneStellen, " +
                                    "ort = @ort, " +
                                    "vorraussetzungen = @vorraussetzungen " +
                                    "WHERE id = @id";
           
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@stellenName", stelle.stellenName);
                cmd.Parameters.AddWithValue("@beschreibung", stelle.beschreibung);
                cmd.Parameters.AddWithValue("@institut", stelle.institut);
                cmd.Parameters.AddWithValue("@anbieterID", stelle.anbieterID);
                cmd.Parameters.AddWithValue("@startanstellung", startAnstellung);
                cmd.Parameters.AddWithValue("@endeAnstellung", endeAnstellung);
                cmd.Parameters.AddWithValue("@bewerbungsfrist", bewerbungsFrist);
                cmd.Parameters.AddWithValue("@monatsStunden", stelle.monatsStunden);
                cmd.Parameters.AddWithValue("@anzahlOffeneStellen", stelle.anzahlOffeneStellen);
                cmd.Parameters.AddWithValue("@ort", stelle.ort);
                cmd.Parameters.AddWithValue("@vorraussetzungen", stelle.vorraussetzungen);
                cmd.Parameters.AddWithValue("@id", stelle.id);

           
                connect();
                cmd.ExecuteNonQuery();
                disconntect();
                return true;

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool stellenangebotHinzufügen(Stellenangebot stelle)
        {
            string startAnstellung = stelle.startAnstellung.getDate();
            string endeAnstellung = stelle.endeAnstellung.getDate();
            string bewerbungsFrist = stelle.bewerbungsFrist.getDate();

            string query = "INSERT INTO Stellenangebote" +
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
                                        "@stellenName, " +
                                        "@beschreibung, " +
                                        "@institut, " +
                                        "@anbieterID, " +
                                        "@startAnstellung, " +
                                        "@endeAnstellung, " +
                                        "@bewerbungsFrist, " +
                                        "@monatsStunden, " +
                                        "@anzahlOffeneStellen, " +
                                        "@ort, " +
                                        "@vorraussetzungen " + ") ";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@stellenName", stelle.stellenName);
                cmd.Parameters.AddWithValue("@beschreibung", stelle.beschreibung);
                cmd.Parameters.AddWithValue("@institut", stelle.institut);
                cmd.Parameters.AddWithValue("@anbieterID", stelle.anbieterID);
                cmd.Parameters.AddWithValue("@startanstellung", startAnstellung);
                cmd.Parameters.AddWithValue("@endeAnstellung", endeAnstellung);
                cmd.Parameters.AddWithValue("@bewerbungsfrist", bewerbungsFrist);
                cmd.Parameters.AddWithValue("@monatsStunden", stelle.monatsStunden);
                cmd.Parameters.AddWithValue("@anzahlOffeneStellen", stelle.anzahlOffeneStellen);
                cmd.Parameters.AddWithValue("@ort", stelle.ort);
                cmd.Parameters.AddWithValue("@vorraussetzungen", stelle.vorraussetzungen);



                connect();
                cmd.ExecuteNonQuery();
                disconntect();
                return true;

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool stellenangebotLoeschen(Stellenangebot stelle){
            string query = "DELETE FROM Stellenangebote " +
                            "WHERE id = @id";
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", stelle.id);

                connect();
                cmd.ExecuteNonQuery();
                disconntect();
                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public Stellenangebot stellenangebotLesen(int stellenID)
        {
            Stellenangebot stelle = new Stellenangebot();
            string dateformat = "dd-MM-yyyy";

            string query = "SELECT stellenName, " +
                "beschreibung, institut, anbieterID, " +
                "startAnstellung, endeAnstellung, " +
                "bewerbungsFrist, monatsStunden, " +
            "anzahlOffeneStellen, ort, vorraussetzungen " +
            "FROM Stellenangebote WHERE id= @id";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", stellenID);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
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
                    stelle.id = stellenID;


                }

                reader.Close();
                disconntect();
                return stelle;
            }
            catch (SqlException e)
            {
                return null;
            }
        }

        public LinkedList<Stellenangebot> stellenangebotUebersichtLesen(int anbieterID)
        {
            LinkedList<Stellenangebot> liste = new LinkedList<Stellenangebot>();
            string dateformat = "dd-MM-yyyy";

            string query = "SELECT stellenName, " +
                "beschreibung, institut, anbieterID, " +
                "startAnstellung, endeAnstellung, " +
                "bewerbungsFrist, monatsStunden, " +
            "anzahlOffeneStellen, ort, vorraussetzungen, id " +
            "FROM Stellenangebote WHERE anbieterID= @anbieterID";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@anbieterID", anbieterID);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Stellenangebot stelle = new Stellenangebot();
                        stelle.stellenName = reader.GetValue(0).ToString();
                        stelle.beschreibung = reader.GetValue(1).ToString();
                        stelle.institut = reader.GetValue(2).ToString();
                        stelle.anbieterID = anbieterID;
                        stelle.startAnstellung = new Date(reader.GetDateTime(4).ToString(dateformat));
                        stelle.endeAnstellung = new Date(reader.GetDateTime(5).ToString(dateformat));
                        stelle.bewerbungsFrist = new Date(reader.GetDateTime(6).ToString(dateformat));
                        stelle.monatsStunden = Convert.ToInt32(reader.GetValue(7));
                        stelle.anzahlOffeneStellen = Convert.ToInt32(reader.GetValue(8));
                        stelle.ort = reader.GetValue(9).ToString();
                        stelle.vorraussetzungen = reader.GetValue(10).ToString();
                        stelle.id = Convert.ToInt32(reader.GetValue(11));

                        liste.AddLast(stelle);
                    }

                }

                reader.Close();
                disconntect();
                return liste;
            }
            catch (SqlException e)
            {
                return null;
            }
        }
    }
}