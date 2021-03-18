using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class TiposEnvases
    {
        public TiposEnvases()
        {
            EnvasesPrimarios = new HashSet<EnvasesPrimarios>();
        }
        public int IdTipoEnvases { get; set; }
        public string Descripcion { get; set; }
        public ICollection<EnvasesPrimarios> EnvasesPrimarios { get; set; }
    }
}

