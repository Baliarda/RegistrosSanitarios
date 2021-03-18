using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class ValoresConcentracion
    {
        public ValoresConcentracion() { }
        public int IdvaloresConcentracion { get; set; }
        public int IdPrincipiosActivosRegistrosSanit { get; set; }
        public int IdConcentraciones { get; set; }
        public string ValorConcentracion { get; set; }
        public Concentraciones IdConcentracionesNavigation { get; set; }
        public PrincipiosActivosRegistrosSanit IdPrincipiosActivosRegistrosSanitNavigation { get; set; }
    }
}

