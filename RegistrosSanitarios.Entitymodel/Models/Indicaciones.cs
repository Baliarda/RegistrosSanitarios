using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class Indicaciones
    {
        public Indicaciones()
        {
            IndicacionesProductos = new HashSet<IndicacionesProductos>();
        }
        public int IdIndicaciones { get; set; }
        public string Descripcion { get; set; }
        public ICollection<IndicacionesProductos> IndicacionesProductos { get; set; }
    }
}

