using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class FormasFarmaceuticas
    {
        public FormasFarmaceuticas()
        {
            ProductosRegistrosSanit = new HashSet<ProductosRegistrosSanit>();
        }
        public int IdFormasFarmaceuticas { get; set; }
        public string Descripcion { get; set; }
        public ICollection<ProductosRegistrosSanit> ProductosRegistrosSanit { get; set; }
    }
}
