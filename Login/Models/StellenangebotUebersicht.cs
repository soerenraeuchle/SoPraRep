using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login.Models
{
    public class StellenangebotUebersicht
    {
        List<Stellenangebot> angebote;

        public StellenangebotUebersicht(List<Stellenangebot> _angebote)
        {
            this.angebote = _angebote;
        }
    }
}