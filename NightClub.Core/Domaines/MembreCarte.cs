using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Core.Domaines
{
    public class MembreCarte
    {
        public int Id { get; set; }
        public Membre Membre { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
