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
            string[] splitDate = date.Split('-');
            this.day = Convert.ToInt32(splitDate[0]);
            this.month = Convert.ToInt32(splitDate[1]);
            this.year = Convert.ToInt32(splitDate[2]);
        }

        public string getDate(){
            string date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            return date;
        }

        

    }
}