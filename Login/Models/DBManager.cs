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

        //private static string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\dev\\asp\\sopra\\Login\\Login\\App_Data\\Sopra.mdf;Integrated Security=True;User Instance=True";
        private static string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|Sopra.mdf;Integrated Security=True;User Instance=True";
        private SqlConnection con = new SqlConnection(ConnectionString);

        private string lastSqlQuery;
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

        public Benutzer benutzerAuslesen(String email)
        {
            Benutzer benutzer = new Benutzer();
            string query = "SELECT vorname, nachname, rechte, passwort FROM Benutzer WHERE email=@email";

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
                    return true;
                }
                else
                {
                    disconnect();
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
            string query = "SELECT vorname, nachname, strasse, hausnummer, plz, wohnort, matrikelnummer, studiengang, fachsemester, passwort FROM Benutzer WHERE email=@email";

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
            string query = "SELECT vorname, nachname, institut, passwort FROM Benutzer WHERE email=@email";

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

                benutzer.passwort = reader.GetString(3);
                benutzer.confirmPasswort = benutzer.passwort;

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
                                "institut=@institut " +
                            "WHERE email=@email";

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vorname", benutzer.vorname);
                cmd.Parameters.AddWithValue("@nachname", benutzer.nachname);

                cmd.Parameters.AddWithValue("@institut", benutzer.institut);

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
            try
            {
                lastSqlQuery = query;
                if (!connect()) return -1;
                SqlCommand cmd = new SqlCommand(query, con);
                int affectedRows = cmd.ExecuteNonQuery();
                if (!disconnect()) return -1;
                return affectedRows;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return -1;
            }
        }


        //public int aendernPrepared(string query, Object[] values)
        //{
        //    try
        //    {
        //        lastSqlQuery = query;
        //        if (!connect()) return -1;
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Prepare();
        //        for (int i = 0; i < values.Count(); i++)
        //        {
        //            Type typ = values[i].GetType();

        //            //If TypeOf a Is CBackware Then

        //            if (typ.IsInstanceOfType(string)) {

        //            }

        //            if (typ.IsInstanceOfType(int)) {

        //            }
        //            if (typ.IsInstanceOfType(bool)) {

        //            }

        //            cmd.Parameters.Add(values[i]);
        //        }

        //        int affectedRows = cmd.ExecuteNonQuery();
        //        if (!disconnect()) return -1;
        //        return affectedRows;
        //    }
        //    catch (SqlException e)
        //    {
        //        lastError = e.StackTrace;
        //        Console.WriteLine(e.StackTrace);
        //        return -1;
        //    }
        //}
    }
}