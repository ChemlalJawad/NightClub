using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NightClub.Core.Domaines
{
    public class Membre
    {
        public int Id { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool IsBlacklister { get; set; }
        public DateTime DebutDateBlacklister { get; set; }
        public DateTime FinDateBlacklister { get; set; }
        public ICollection<IDCarte> CarteIdentites { get; set; } = new List<IDCarte>();
        public ICollection<MembreCarte> MembreCartes { get; set; } = new List<MembreCarte>();

    }
}
