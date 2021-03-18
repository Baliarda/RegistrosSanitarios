using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class TiposCircuitos
    {
        public TiposCircuitos()
        {
            Circuitos = new HashSet<Circuitos>();
        }
        public int IdtiposCircuitos { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Circuitos> Circuitos { get; set; }
    }
}

