using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class PrincipiosActivosRegistrosSanit
    {
        public PrincipiosActivosRegistrosSanit()
        {
            ValoresConcentracion = new HashSet<ValoresConcentracion>();
        }
        public int IdPrincipiosActivosRegistrosSanit { get; set; }
        public int IdRegistrosSanitarios { get; set; }
        public int IdPrincipiosActivos { get; set; }
        public PrincipiosActivos IdPrincipiosActivosNavigation { get; set; }
        public ICollection<ValoresConcentracion> ValoresConcentracion { get; set; }
    }
}

