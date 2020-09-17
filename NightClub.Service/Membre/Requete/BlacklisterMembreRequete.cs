using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Service.Membre.Requete
{
    public class BlacklisterMembreRequete
    {
        public int MembreId { get; set; }
        public DateTime DebutDateBlacklister { get; set; }
        public DateTime FinDateBlacklister { get; set; }
    }
}
