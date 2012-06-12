using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login.Models
{
    public class BenutzerUebersicht
    {
        public LinkedList<Benutzer> benutzerList {get;set;}

        public BenutzerUebersicht(LinkedList<Benutzer> benutzerList)
        {
            this.benutzerList = benutzerList;
        }

        public BenutzerUebersicht()
        {
            benutzerList = null;
        }
    }
}