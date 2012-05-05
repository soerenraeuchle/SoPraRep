using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;




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
    }
}