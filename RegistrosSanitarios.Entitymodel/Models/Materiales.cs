using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class Materiales
    {
        public Materiales()
        {
            EnvasesPrimarios = new HashSet<EnvasesPrimarios>();
        }
        public int IdMateriales { get; set; }
        public string Descripcion { get; set; }
        public ICollection<EnvasesPrimarios> EnvasesPrimarios { get; set; }
    }
}

