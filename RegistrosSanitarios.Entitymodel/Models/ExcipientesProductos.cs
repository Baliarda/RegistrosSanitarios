using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class ExcipientesProductos
    {
        public ExcipientesProductos() { }
        public int IdExcipientesProductos { get; set; }
        public int IdExcipientes { get; set; }
        public int IdProductosRegistrosSanit { get; set; }
        public Excipientes IdExcipientesNavigation { get; set; }
        public ProductosRegistrosSanit IdProductosRegistrosSanitNavigation { get; set; }
    }
}
