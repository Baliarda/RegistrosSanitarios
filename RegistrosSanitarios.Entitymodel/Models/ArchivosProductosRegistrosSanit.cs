using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class ArchivosProductosRegistrosSanit
    {
        public ArchivosProductosRegistrosSanit() { }
        public int IdArchivosProductosRegistrosSanit { get; set; }
        public int IdProductosRegistrosSanit { get; set; }
        public string Archivo { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }
        public ProductosRegistrosSanit IdProductosRegistrosSanitNavigation { get; set; }
    }
}


