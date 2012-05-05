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
        public int day {  get; private set; }
        public int month { get; private set; }
        public int year { get; private set; }

<<<<<<< HEAD

        public Date(int _day, int _month, int _year)
        {
            this.day = _day;
            this.month = _month;
            this.year = _year;
        }

=======
>>>>>>> 432761c... Stellenangebot erstellen und anzeigen gefixt, Date hat nun Konstruktor (reverse-merged from commit 8d30e9d4c2441c18b63a7c9ba248095f61c094b5)
        public string getDate(){
            string date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            return date;
        }

<<<<<<< HEAD
=======
        public void setDate(string date)
        {
            string[] splitDate = date.Split('-');
            day = Convert.ToInt32(splitDate[0]);
            month = Convert.ToInt32(splitDate[0]);
            year = Convert.ToInt32(splitDate[0]);
        }

        

>>>>>>> 432761c... Stellenangebot erstellen und anzeigen gefixt, Date hat nun Konstruktor (reverse-merged from commit 8d30e9d4c2441c18b63a7c9ba248095f61c094b5)
    }
}