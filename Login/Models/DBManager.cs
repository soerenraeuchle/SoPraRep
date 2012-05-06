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

        /// <summary>
        /// Führt einen SQL Befehl aus, der Zeilen zurückgibt.
        /// </summary>
        /// <param name="query">
        /// SQL Befehl
        /// </param>
        /// <returns>
        /// Gibt ein SqlDataReader Objekt zurück, das die Daten des Querys enthält. Im Fehlerfall wird null zurückgegeben.
        /// </returns>
        public ArrayList auslesen(string query)
        {
            
            try
            {
                lastSqlQuery = query;
                ArrayList daten = new ArrayList();
                if (!connect()) return null;
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read()) 
                    {
                        int columns = reader.FieldCount;

                        ArrayList data = new ArrayList();

                        for (int i = 0; i < columns; i++)
                        {
                            var tmp = reader.GetValue(i);
                            data.Add(tmp);
                        }

                        daten.Add(data);

                    }
                }
                reader.Close();

                if (!disconnect()) return null;
                return daten;
            }
            catch (SqlException e)
            {
                lastError = e.StackTrace;
                Console.WriteLine(e.StackTrace);
                return null;
            }
            catch (InvalidOperationException e)
            {
                lastError = e.StackTrace;
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
    }
}