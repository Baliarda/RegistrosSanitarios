using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class TareasProductosRegistrosSanit
    {
        public TareasProductosRegistrosSanit() { }
        public int IdTarea { get; set; }
        public int IdProductosRegistrosSanit { get; set; }
        public int IdCircuitos { get; set; }
        public Circuitos IdCircuitosNavigation { get; set; }
        public ProductosRegistrosSanit IdProductosRegistrosSanitNavigation { get; set; }
    }
}
