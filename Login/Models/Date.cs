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


        public Date(int _day, int _month, int _year)
        {
            this.day = _day;
            this.month = _month;
            this.year = _year;
        }

        public string getDate(){
            string date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            return date;
        }

    }
}