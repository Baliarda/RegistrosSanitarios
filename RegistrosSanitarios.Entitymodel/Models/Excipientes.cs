using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class Excipientes
    {
        public Excipientes()
        {
            ExcipientesProductos = new HashSet<ExcipientesProductos>();
        }
        public int IdExcipientes { get; set; }
        public string Descripcion { get; set; }
        public ICollection<ExcipientesProductos> ExcipientesProductos { get; set; }
    }
}
