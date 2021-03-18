using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class PrincipiosActivos
    {
        public PrincipiosActivos()
        {
            PrincipiosActivosRegistrosSanit = new HashSet<PrincipiosActivosRegistrosSanit>();
        }
        public int IdPrincipiosActivos { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public string DescripcionIngles { get; set; }
        public int AplicacionId { get; set; }
        public int? IdPrincipiosActivosSitio { get; set; }
        public string IdPrincipiosActivosANMAT { get; set; }
        public ICollection<PrincipiosActivosRegistrosSanit> PrincipiosActivosRegistrosSanit { get; set; }
    }
}

