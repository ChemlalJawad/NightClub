using NightClub.Service.Membre.Requete;
using NightClub.Core.Domaines;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Service.Membre
{
    public interface IMembreService
    {
        Core.Domaines.Membre CreerMembre(CreerMembreRequete requete);
        Core.Domaines.Membre BlacklisterMembre(BlacklisterMembreRequete requete);
        Core.Domaines.Membre ModifierMembre(ModifierMembreRequete requete);
        Core.Domaines.Membre GenerNouvelleCarteMembre(int membreId);
        List<Core.Domaines.Membre> RecupererMembreBlacklister();
    }
}
