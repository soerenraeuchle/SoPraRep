using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login.Models
{
    public class StellenangebotUebersicht
    {
        public LinkedList<Stellenangebot> angebote {get;set;}

        public StellenangebotUebersicht(LinkedList<Stellenangebot> _angebote)
        {
            this.angebote = _angebote;
        }

        public StellenangebotUebersicht()
        {
            angebote = null;
        }
    }
}