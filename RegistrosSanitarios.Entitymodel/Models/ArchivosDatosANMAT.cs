using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class ArchivosDatosANMAT
    {
        public ArchivosDatosANMAT() { }
        public int IdarchivosDatosANMAT { get; set; }
        public int IdDatosANMAT { get; set; }
        public string Archivo { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }
        public DatosANMAT IdDatosANMATNavigation { get; set; }
    }
}

