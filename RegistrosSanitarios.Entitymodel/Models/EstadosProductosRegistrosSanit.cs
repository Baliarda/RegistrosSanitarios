using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class EstadosProductosRegistrosSanit
    {
        public EstadosProductosRegistrosSanit()
        {
            ProductosRegistrosSanit = new HashSet<ProductosRegistrosSanit>();
        }
        public int IdEstadosProductosRegistrosSanit { get; set; }
        public string Descripcion { get; set; }
        public ICollection<ProductosRegistrosSanit> ProductosRegistrosSanit { get; set; }
    }
}


