using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login.Models
{

    /// <summary>
    /// Die Date Klasse repräsentiert das Datumsformat für die Datenbank
    /// </summary>
    public class Date
    {
        public int day {  get; set; }
        public int month { get; set; }
        public int year { get; set; }

        public Date(string date)
        {
            setDate(date);
        }

        public Date()
        {
            
        }

        public string getDate(){
            string date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            return date;
        }

        public void setDate(string date)
        {
            string[] splitDate = date.Split('-');
            day = Convert.ToInt32(splitDate[0]);
            month = Convert.ToInt32(splitDate[1]);
            year = Convert.ToInt32(splitDate[2]);
        }

        

    }
}