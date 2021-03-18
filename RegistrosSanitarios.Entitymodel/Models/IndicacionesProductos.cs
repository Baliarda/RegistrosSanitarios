using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class IndicacionesProductos
    {
        public IndicacionesProductos() { }
        public int IdIndicacionesProductos { get; set; }
        public int IdIndicaciones { get; set; }
        public int IdProductosRegistrosSanit { get; set; }
        public Indicaciones IdIndicacionesNavigation { get; set; }
        public ProductosRegistrosSanit IdProductosRegistrosSanitNavigation { get; set; }
    }
}

