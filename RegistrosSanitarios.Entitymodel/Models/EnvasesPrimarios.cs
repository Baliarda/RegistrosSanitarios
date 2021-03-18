using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class EnvasesPrimarios
    {
        public EnvasesPrimarios()
        {
            ProductosRegistrosSanit = new HashSet<ProductosRegistrosSanit>();
        }
        public int IdEnvasesPrimarios { get; set; }
        public int IdMateriales { get; set; }
        public int IdTipoEnvases { get; set; }
        public Materiales IdMaterialesNavigation { get; set; }
        public TiposEnvases IdTipoEnvasesNavigation { get; set; }
        public ICollection<ProductosRegistrosSanit> ProductosRegistrosSanit { get; set; }
    }
}

