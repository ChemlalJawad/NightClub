using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Core.Domaines
{
    public class IDCarte
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateNaissance { get; set; }
        public string RegistreNational { get; set; }
        public DateTime DateValidation { get; set; }
        public DateTime DateExpiration { get; set; }
        public int NumeroCarte { get; set; }

        public int CalculerAge(DateTime dateNaissance)
        {
            return DateTime.Now.Year - dateNaissance.Year -
                   (DateTime.Now.Month <= dateNaissance.Month ?
                   (DateTime.Now.Day < dateNaissance.Day ? 1 : 0) : 0);
        }
    }
}
