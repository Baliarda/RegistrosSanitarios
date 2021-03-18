using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class EstadosCircuitos
    {
        public EstadosCircuitos()
        {
            Circuitos = new HashSet<Circuitos>();
        }
        public int IdestadosCircuitos { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Circuitos> Circuitos { get; set; }
    }
}

