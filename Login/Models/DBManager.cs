using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Security;
using System.Web.Mvc;




namespace Login.Models
{
    /// <summary>
    /// Eine Singleton Klasse, die die Verbindung zur Datenbank enthält und SQL Befehle ausführen kann.
    /// </summary>
    public class DBManager
    {
        private DBManager() { }
        private static DBManager instance = null;

        private static string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|Sopra.mdf;Integrated Security=True;User Instance=True";
        private SqlConnection con = new SqlConnection(ConnectionString);

        private string lastError;

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
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
            catch (InvalidOperationException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        private bool disconnect()
        {
            try
            {
                con.Close();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

      
        //------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------Benutzer Datenbank Methoden--------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------

        public Admin adminAuslesen(String email)
        {
            Admin benutzer = new Admin();
            string query = "SELECT vorname, nachname, rechte, passwort, id FROM Benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    reader.Read();

                    //Anbieter auslesen
                    benutzer.vorname = reader.GetString(0);
                    benutzer.nachname = reader.GetString(1);
                    benutzer.rechte = reader.GetInt32(2);
                    benutzer.passwort = reader.GetString(3);
                    benutzer.confirmPasswort = benutzer.passwort;
                    benutzer.id = reader.GetInt32(4);
                    benutzer.email = email;

                    reader.Close();
                    disconnect();
                    return benutzer;
                }
                else
                {
                    reader.Close();
                    disconnect();
                    return benutzer;
                }
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

       

        public Benutzer benutzerAuslesen(String email)
        {
            Benutzer benutzer = new Benutzer();
            string query = "SELECT vorname, nachname, rechte, passwort, id FROM Benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    reader.Read();

                    //Anbieter auslesen
                    benutzer.vorname = reader.GetString(0);
                    benutzer.nachname = reader.GetString(1);
                    benutzer.rechte = reader.GetInt32(2);
                    benutzer.passwort = reader.GetString(3);
                    benutzer.confirmPasswort = benutzer.passwort;
                    benutzer.id = reader.GetInt32(4);
                    benutzer.email = email;

                    reader.Close();
                    disconnect();
                    return benutzer;
                }
                else
                {
                    reader.Close();
                    disconnect();
                    return benutzer;
                }
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Liest die Rechte für einen Benutzer aus.
        /// </summary>
        /// <param name="email">Emailadresse des Benutzers</param>
        /// <returns></returns>
        public int rechteFuerBenutzer(String email)
        {
            int rechte = -1;
            String query = "SELECT rechte FROM benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                //Bewerber auslesen
                rechte = reader.GetInt32(0);

                reader.Close();
                disconnect();
                return rechte;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return rechte;
            }
        }

        //Email Validierung
        public bool emailVorhanden(String email)
        {
            string query = "SELECT id FROM Benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    disconnect();
                    reader.Close();
                    return true;
                }
                else
                {
                    disconnect();
                    reader.Close();
                    return false;
                }
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                
                return false;
            }
        }


        //---------------------------------------- Bewerber ------------------------------------------------------

        /// <summary>
        /// Speichert einen Benutzer in der Datenbank.
        /// </summary>
        /// <param name="benutzer">Der Benutzer, der gespeichert werden soll.</param>
        /// <returns>Gibt true zurück, falls der Benutzer gespeichert wurde, andernfalls false.</returns>
        public bool bewerberSpeichern(Bewerber benutzer)
        {
            benutzer.passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(benutzer.passwort, "SHA1");

            string query = "INSERT INTO " +
                                "Benutzer " +
                                    "(" +
                                        "vorname, " +
                                        "nachname, " +

                                        "email, " +
                                        "passwort, " +

                                        "rechte, " +
                                        "freischaltung, " +

                                        "studiengang, " +
                                        "fachsemester, " +

                                        "strasse, " +
                                        "hausnummer, " +
                                        "plz, " +
                                        "wohnort, " +

                                        "matrikelnummer " +
                                    ") " +
                                "VALUES " +
                                    "(" +
                                        "@vorname, " +
                                        "@nachname, " +

                                        "@email, " +
                                        "@passwort, " +

                                        "@rechte, " +
                                        "@freischaltung, " +

                                        "@studiengang, " +
                                        "@fachsemester, " +

                                        "@strasse, " +
                                        "@hausnummer, " +
                                        "@plz, " +
                                        "@wohnort, " +

                                        "@matrikelnummer" +

                                    ")";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);
                cmd.Parameters.AddWithValue("@email", benutzer.email);
                cmd.Parameters.AddWithValue("@passwort", benutzer.passwort);
                cmd.Parameters.AddWithValue("@rechte", benutzer.rechte);
                cmd.Parameters.AddWithValue("@freischaltung", benutzer.freischaltung);
                cmd.Parameters.AddWithValue("@studiengang", benutzer.studiengang);
                cmd.Parameters.AddWithValue("@fachsemester", benutzer.fachsemester);
                cmd.Parameters.AddWithValue("@strasse", benutzer.strasse);
                cmd.Parameters.AddWithValue("@hausnummer", benutzer.hausnummer);
                cmd.Parameters.AddWithValue("@plz", benutzer.plz);
                cmd.Parameters.AddWithValue("@wohnort", benutzer.wohnort);
                cmd.Parameters.AddWithValue("@matrikelnummer", benutzer.matrikelnummer);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        public Bewerber bewerberAuslesen(String email)
        {
            Bewerber benutzer = new Bewerber();
            string query = "SELECT vorname, nachname, strasse, hausnummer, plz, wohnort, matrikelnummer, studiengang, fachsemester, passwort, id FROM Benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                //Bewerber auslesen
                benutzer.vorname = reader.GetString(0);
                benutzer.nachname = reader.GetString(1);
                benutzer.strasse = reader.GetString(2);
                benutzer.hausnummer = reader.GetString(3);
                benutzer.plz = reader.GetInt32(4);
                benutzer.wohnort = reader.GetString(5);
                benutzer.matrikelnummer = reader.GetInt32(6);
                benutzer.studiengang = reader.GetString(7);
                benutzer.fachsemester = reader.GetInt32(8);
                

                benutzer.email = email;

                benutzer.passwort = reader.GetString(9);
                benutzer.confirmPasswort = benutzer.passwort;

                benutzer.id = reader.GetInt32(10);

                reader.Close();
                disconnect();
                return benutzer;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }


        public bool bewerberAktualisieren(Bewerber benutzer)
        {
            string query = "UPDATE Benutzer SET " +
                                "vorname=@vorname, " +
                                "nachname=@nachname, " +
                                "strasse=@strasse, " +
                                "hausnummer=@hausnummer, " +
                                "plz=@plz, " +
                                "wohnort=@wohnort, " +
                                "matrikelnummer=@matrikelnummer, " +
                                "studiengang=@studiengang, " +
                                "fachsemester=@fachsemester " +
                            "WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);
                cmd.Parameters.AddWithValue("@strasse", benutzer.strasse);
                cmd.Parameters.AddWithValue("@hausnummer", benutzer.hausnummer);
                cmd.Parameters.AddWithValue("@plz", benutzer.plz);
                cmd.Parameters.AddWithValue("@wohnort", benutzer.wohnort);
                
                cmd.Parameters.AddWithValue("@matrikelnummer", benutzer.matrikelnummer);
                cmd.Parameters.AddWithValue("@studiengang", benutzer.studiengang);
                cmd.Parameters.AddWithValue("@fachsemester", benutzer.fachsemester);

                cmd.Parameters.AddWithValue("@email", benutzer.email);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        //---------------------------------------- Anbieter ------------------------------------------------------
        /// <summary>
        /// Speichert einen neuen Anbieter in der Datenbank.
        /// </summary>
        /// <param name="benutzer">Der neue Anbieter.</param>
        /// <returns>Gibt true zurück, falls kein Fehler auftrat, andernfalls false.</returns>
        public bool anbieterSpeichern(Anbieter benutzer)
        {
            benutzer.passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(benutzer.passwort, "SHA1");

            string query = "INSERT INTO " +
                                "Benutzer " +
                                    "(" +
                                        "vorname, " +
                                        "nachname, " +

                                        "email, " +
                                        "passwort, " +

                                        "rechte, " +
                                        "freischaltung, " +

                                        "institut, " +
                                        "stellvertreterID" +
                                    ") " +
                                "VALUES " +
                                    "(" +
                                        "@vorname, " +
                                        "@nachname, " +

                                        "@email, " +
                                        "@passwort, " +

                                        "@rechte, " +
                                        "@freischaltung, " +
                
                                        "@institut, " +
                                        "@stellvertreterID" +
                                    ")";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);
                cmd.Parameters.AddWithValue("@email", benutzer.email);
                cmd.Parameters.AddWithValue("@passwort", benutzer.passwort);
                cmd.Parameters.AddWithValue("@rechte", benutzer.rechte);
                cmd.Parameters.AddWithValue("@freischaltung", benutzer.freischaltung);
                cmd.Parameters.AddWithValue("@institut", benutzer.institut);
                cmd.Parameters.AddWithValue("@stellvertreterID", benutzer.stellvertreterID);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        public Anbieter anbieterAuslesen(String email)
        {
            Anbieter benutzer = new Anbieter();
            string query = "SELECT vorname, nachname, institut, stellvertreterID, passwort, id FROM Benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    //Anbieter auslesen
                    benutzer.vorname = reader.GetString(0);
                    benutzer.nachname = reader.GetString(1);
                    benutzer.institut = reader.GetString(2);
                    benutzer.stellvertreterID = reader.GetInt32(3);

                    benutzer.passwort = reader.GetString(4);
                    benutzer.confirmPasswort = benutzer.passwort;

                    benutzer.id = reader.GetInt32(5);

                    benutzer.email = email;

                    reader.Close();
                
                disconnect();
                return benutzer;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }


        public bool anbieterAktualisieren(Anbieter benutzer)
        {
            string query = "UPDATE Benutzer SET " +
                                "vorname=@vorname, " +
                                "nachname=@nachname, " +
                                "institut=@institut, " +
                                "stellvertreterID=@stellvertreterID " +
                            "WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);

                cmd.Parameters.AddWithValue("@institut", benutzer.institut);
                cmd.Parameters.AddWithValue("@stellvertreterID", benutzer.stellvertreterID);

                cmd.Parameters.AddWithValue("@email", benutzer.email);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        //---------------------------------------- Bearbeiter ------------------------------------------------------

        /// <summary>
        /// Speichert einen neuen Anbieter in der Datenbank.
        /// </summary>
        /// <param name="benutzer">Der neue Anbieter.</param>
        /// <returns>Gibt true zurück, falls kein Fehler auftrat, andernfalls false.</returns>
        public bool bearbeiterSpeichern(Bearbeiter benutzer)
        {
            benutzer.passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(benutzer.passwort, "SHA1");

            string query = "INSERT INTO " +
                                "Benutzer " +
                                    "(" +
                                        "vorname, " +
                                        "nachname, " +

                                        "email, " +
                                        "passwort, " +

                                        "rechte, " +
                                        "freischaltung" +
                                    ") " +
                                "VALUES " +
                                    "(" +
                                        "@vorname, " +
                                        "@nachname, " +

                                        "@email, " +
                                        "@passwort, " +

                                        "@rechte, " +
                                        "@freischaltung" +
                                    ")";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);
                cmd.Parameters.AddWithValue("@email", benutzer.email);
                cmd.Parameters.AddWithValue("@passwort", benutzer.passwort);
                cmd.Parameters.AddWithValue("@rechte", benutzer.rechte);
                cmd.Parameters.AddWithValue("@freischaltung", benutzer.freischaltung);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public Bearbeiter bearbeiterAuslesen(String email)
        {
            Bearbeiter benutzer = new Bearbeiter();
            string query = "SELECT vorname, nachname, passwort, id FROM Benutzer WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@email", email);

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                benutzer.vorname = reader.GetString(0);
                benutzer.nachname = reader.GetString(1);

                benutzer.passwort = reader.GetString(2);
                benutzer.confirmPasswort = benutzer.passwort;

                benutzer.id = reader.GetInt32(3);

                benutzer.email = email;

                reader.Close();

                disconnect();
                return benutzer;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }
       


        public bool bearbeiterAktualisieren(Bearbeiter benutzer)
        {
            string query = "UPDATE Benutzer SET " +
                                "vorname=@vorname, " +
                                "nachname=@nachname " +
                            "WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);

                cmd.Parameters.AddWithValue("@email", benutzer.email);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        //--------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------- ADMIN ----------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------

        public bool adminAnlegen(Admin benutzer)
        {
            benutzer.passwort = FormsAuthentication.HashPasswordForStoringInConfigFile(benutzer.passwort, "SHA1");

            string query = "INSERT INTO " +
                                "Benutzer " +
                                    "(" +
                                        "vorname, " +
                                        "nachname, " +
                                    
                                        "email, " +
                                        "passwort, " +

                                        "rechte, " +
                                        "freischaltung" +
                                    ") " +
                                "VALUES " +
                                    "(" +
                                        "@vorname, " +
                                        "@nachname, " +

                                        "@email, " +
                                        "@passwort, " +

                                        "@rechte, " +
                                        "@freischaltung " +
                                    ")";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);

                cmd.Parameters.AddWithValue("@email", benutzer.email);
                cmd.Parameters.AddWithValue("@passwort", benutzer.passwort);
                cmd.Parameters.AddWithValue("@rechte", benutzer.rechte);
                cmd.Parameters.AddWithValue("@freischaltung", benutzer.freischaltung);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool benutzerLoeschen(int benutzerID)
        {
            string query = "DELETE FROM benutzer WHERE id=@id";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@id", benutzerID);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        public bool benutzerFreischalten(int benutzerID)
        {
            string query = "UPDATE benutzer SET freischaltung='True' WHERE id=@id";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@id", benutzerID);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        public bool benutzerSperren(int benutzerID)
        {
            string query = "UPDATE benutzer SET freischaltung='False' WHERE id=@id";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@id", benutzerID);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        public LinkedList<Benutzer> benutzerAuslesen()
        {
            LinkedList<Benutzer> benutzerList = new LinkedList<Benutzer>();

            string query = "SELECT id, vorname, nachname, email, studiengang, fachsemester, strasse, hausnummer, plz, wohnort, passwort, rechte, freischaltung, matrikelnummer, institut, stellvertreterID " +
                            "FROM Benutzer ORDER BY nachname ASC, vorname ASC";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int rechte = reader.GetInt32(11);

                        //Benutzer je nach Rolle einteilen.
                        if (rechte == 1)
                        {
                            //Anbieter
                            Anbieter benutzer = new Anbieter();

                            benutzer.id = reader.GetInt32(0);
                            benutzer.vorname = reader.GetString(1);
                            benutzer.nachname = reader.GetString(2);
                            benutzer.email = reader.GetString(3);
                            benutzer.passwort = reader.GetString(10);
                            benutzer.confirmPasswort = benutzer.passwort;
                            benutzer.rechte = rechte;
                            benutzer.freischaltung = reader.GetBoolean(12);

                            benutzer.institut = reader.GetString(14);
                            benutzer.stellvertreterID = reader.GetInt32(15);

                            benutzerList.AddLast(benutzer);
                        }
                        else if (rechte == 2)
                        {
                            //Bearbeiter
                            Bearbeiter benutzer = new Bearbeiter();

                            benutzer.id = reader.GetInt32(0);
                            benutzer.vorname = reader.GetString(1);
                            benutzer.nachname = reader.GetString(2);
                            benutzer.email = reader.GetString(3);
                            benutzer.passwort = reader.GetString(10);
                            benutzer.confirmPasswort = benutzer.passwort;
                            benutzer.rechte = rechte;
                            benutzer.freischaltung = reader.GetBoolean(12);

                            benutzerList.AddLast(benutzer);
                        }
                        else if (rechte == 3)
                        {
                            //Admin
                            Admin benutzer = new Admin();

                            benutzer.id = reader.GetInt32(0);
                            benutzer.vorname = reader.GetString(1);
                            benutzer.nachname = reader.GetString(2);
                            benutzer.email = reader.GetString(3);
                            benutzer.passwort = reader.GetString(10);
                            benutzer.confirmPasswort = benutzer.passwort;
                            benutzer.rechte = rechte;
                            benutzer.freischaltung = reader.GetBoolean(12);

                            benutzerList.AddLast(benutzer);
                        }
                        else
                        {
                            //Bewerber
                            Bewerber benutzer = new Bewerber();

                            benutzer.id = reader.GetInt32(0);
                            benutzer.vorname = reader.GetString(1);
                            benutzer.nachname = reader.GetString(2);
                            benutzer.email = reader.GetString(3);
                            benutzer.passwort = reader.GetString(10);
                            benutzer.confirmPasswort = benutzer.passwort;
                            benutzer.rechte = rechte;
                            benutzer.freischaltung = reader.GetBoolean(12);

                            benutzer.strasse = reader.GetString(6);
                            benutzer.hausnummer = reader.GetString(7);
                            benutzer.plz = reader.GetInt32(8);
                            benutzer.wohnort = reader.GetString(9);

                            benutzer.matrikelnummer = reader.GetInt32(13);
                            benutzer.fachsemester = reader.GetInt32(5);
                            benutzer.studiengang = reader.GetString(4);

                            benutzerList.AddLast(benutzer);
                        }

                    }

                }

                reader.Close();
                disconnect();
                return benutzerList;
            }
            catch (SqlException e)
            {
                return null;
            }
        }


        //--------------------------------------------------------------------------------------------------------------------
        //-------------------------------------STELLENANGEBOTE PREPARED STATEMENTS--------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------
        public bool stellenangebotAktualisieren(Models.Stellenangebot stelle)
        {

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
                cmd.Parameters.AddWithValue("@startanstellung", stelle.startAnstellung);
                cmd.Parameters.AddWithValue("@endeAnstellung", stelle.endeAnstellung);
                cmd.Parameters.AddWithValue("@bewerbungsfrist", stelle.bewerbungsFrist);
                cmd.Parameters.AddWithValue("@monatsStunden", stelle.monatsStunden);
                cmd.Parameters.AddWithValue("@anzahlOffeneStellen", stelle.anzahlOffeneStellen);
                cmd.Parameters.AddWithValue("@ort", stelle.ort);
                cmd.Parameters.AddWithValue("@vorraussetzungen", stelle.vorraussetzungen);
                cmd.Parameters.AddWithValue("@id", stelle.id);

           
                connect();
                cmd.ExecuteNonQuery();
                disconnect();
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
                cmd.Parameters.AddWithValue("@startanstellung", stelle.startAnstellung);
                cmd.Parameters.AddWithValue("@endeAnstellung", stelle.endeAnstellung);
                cmd.Parameters.AddWithValue("@bewerbungsfrist", stelle.bewerbungsFrist);
                cmd.Parameters.AddWithValue("@monatsStunden", stelle.monatsStunden);
                cmd.Parameters.AddWithValue("@anzahlOffeneStellen", stelle.anzahlOffeneStellen);
                cmd.Parameters.AddWithValue("@ort", stelle.ort);
                cmd.Parameters.AddWithValue("@vorraussetzungen", stelle.vorraussetzungen);



                connect();
                cmd.ExecuteNonQuery();
                disconnect();
                return true;

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool stellenangebotLoeschen(Stellenangebot stelle) {
            string query = "DELETE FROM Stellenangebote " +
                            "WHERE id = @id";
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", stelle.id);

                connect();
                cmd.ExecuteNonQuery();
                disconnect();
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
                    stelle.stellenName = reader.GetString(0).ToString();
                    stelle.beschreibung = reader.GetString(1).ToString();
                    stelle.institut = reader.GetString(2);
                    stelle.anbieterID = reader.GetInt32(3);
                    stelle.startAnstellung = reader.GetDateTime(4);
                    stelle.endeAnstellung = reader.GetDateTime(5);
                    stelle.bewerbungsFrist = reader.GetDateTime(6);
                    stelle.monatsStunden = reader.GetInt32(7);
                    stelle.anzahlOffeneStellen = reader.GetInt32(8);
                    stelle.ort = reader.GetString(9);
                    stelle.vorraussetzungen = reader.GetString(10);
                    stelle.id = stellenID;


                }

                reader.Close();
                disconnect();
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
                        stelle.stellenName = reader.GetString(0);
                        stelle.beschreibung = reader.GetString(1);
                        stelle.institut = reader.GetString(2);
                        stelle.anbieterID = anbieterID;
                        stelle.startAnstellung =reader.GetDateTime(4);
                        stelle.endeAnstellung = reader.GetDateTime(5);
                        stelle.bewerbungsFrist = reader.GetDateTime(6);
                        stelle.monatsStunden = reader.GetInt32(7);
                        stelle.anzahlOffeneStellen = reader.GetInt32(8);
                        stelle.ort = reader.GetValue(9).ToString();
                        stelle.vorraussetzungen = reader.GetString(10);
                        stelle.id = reader.GetInt32(11);

                        liste.AddLast(stelle);
                    }

                }

                reader.Close();
                disconnect();
                return liste;
            }
            catch (SqlException e)
            {
                return null;
            }
        }

        public LinkedList<Stellenangebot> stellenangeboteUebersichtLesen()
        {
            LinkedList<Stellenangebot> liste = new LinkedList<Stellenangebot>();

            string query = "SELECT stellenName, " +
                "beschreibung, institut, anbieterID, " +
                "startAnstellung, endeAnstellung, " +
                "bewerbungsFrist, monatsStunden, " +
            "anzahlOffeneStellen, ort, vorraussetzungen, id " +
            "FROM Stellenangebote ";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();

                connect();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Stellenangebot stelle = new Stellenangebot();
                        stelle.stellenName = reader.GetString(0);
                        stelle.beschreibung = reader.GetString(1);
                        stelle.institut = reader.GetString(2);
                        stelle.anbieterID = reader.GetInt32(3);
                        stelle.startAnstellung = reader.GetDateTime(4);
                        stelle.endeAnstellung = reader.GetDateTime(5);
                        stelle.bewerbungsFrist = reader.GetDateTime(6);
                        stelle.monatsStunden = reader.GetInt32(7);
                        stelle.anzahlOffeneStellen = reader.GetInt32(8);
                        stelle.ort = reader.GetValue(9).ToString();
                        stelle.vorraussetzungen = reader.GetString(10);
                        stelle.id = reader.GetInt32(11);

                        liste.AddLast(stelle);
                    }

                }

                reader.Close();
                disconnect();
                return liste;
            }
            catch (SqlException e)
            {
                return null;
            }
        }
    }
}