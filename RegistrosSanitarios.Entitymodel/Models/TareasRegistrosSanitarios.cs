using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class TareasRegistrosSanitarios
    {
        public TareasRegistrosSanitarios() { }
        public int IdTarea { get; set; }
        public int IdRegistrosSanitarios { get; set; }
        public int IdCircuitos { get; set; }
        public Circuitos IdCircuitosNavigation { get; set; }
    }
}
