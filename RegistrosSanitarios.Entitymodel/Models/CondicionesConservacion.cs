using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class CondicionesConservacion
    {
        public CondicionesConservacion()
        {
            ProductosRegistrosSanit = new HashSet<ProductosRegistrosSanit>();
        }
        public int IdCondicionesConservacion { get; set; }
        public string Descripcion { get; set; }
        public ICollection<ProductosRegistrosSanit> ProductosRegistrosSanit { get; set; }
    }
}

