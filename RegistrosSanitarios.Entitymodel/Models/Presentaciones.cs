using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class Presentaciones
    {
        public Presentaciones()
        {
            ProductosRegistrosSanit = new HashSet<ProductosRegistrosSanit>();
        }
        public int IdPresentaciones { get; set; }
        public string Descripcion { get; set; }
        public ICollection<ProductosRegistrosSanit> ProductosRegistrosSanit { get; set; }
    }
}

