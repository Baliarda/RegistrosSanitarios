using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class Concentraciones
    {
        public Concentraciones()
        {
            ValoresConcentracion = new HashSet<ValoresConcentracion>();
            ProductosRegistrosSanit = new HashSet<ProductosRegistrosSanit>();
        }
        public int IdConcentraciones { get; set; }
        public string Descripcion { get; set; }
        public ICollection<ValoresConcentracion> ValoresConcentracion { get; set; }
        public ICollection<ProductosRegistrosSanit> ProductosRegistrosSanit { get; set; }
    }
}
