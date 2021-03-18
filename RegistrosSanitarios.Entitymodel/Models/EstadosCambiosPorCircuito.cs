using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class EstadosCambiosPorCircuito
    {
        public EstadosCambiosPorCircuito()
        {
            CambiosPorCircuito = new HashSet<CambiosPorCircuito>();
        }
        public int IdEstadosCambiosPorCircuito { get; set; }
        public string Descripcion { get; set; }
        public ICollection<CambiosPorCircuito> CambiosPorCircuito { get; set; }
    }
}


