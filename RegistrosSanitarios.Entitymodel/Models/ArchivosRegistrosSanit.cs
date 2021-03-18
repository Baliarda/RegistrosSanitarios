using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class ArchivosRegistrosSanit
    {
        public ArchivosRegistrosSanit() { }
        public int IdArchivosRegistrosSanit { get; set; }
        public int IdRegistrosSanitarios { get; set; }
        public string Archivo { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
