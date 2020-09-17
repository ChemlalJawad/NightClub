using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Service.Membre.Requete
{
    public class ModifierMembreRequete
    {
        public int MembreId { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public IDCarte CarteIdentite { get; set; }

        public class IDCarte
        {
            public string Prenom { get; set; }
            public string Nom { get; set; }
            public string RegistreNational { get; set; }
            public DateTime DateNaissance { get; set; }
            public DateTime DateValidation { get; set; }
            public DateTime DateExpiration { get; set; }
            public int NumeroCarte { get; set; }
        }
    }
}
