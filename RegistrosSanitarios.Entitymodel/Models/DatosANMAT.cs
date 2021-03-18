using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class DatosANMAT
    {
        public DatosANMAT()
        {
            Circuitos = new HashSet<Circuitos>(); ArchivosDatosANMAT = new HashSet<ArchivosDatosANMAT>();
        }
        public int IdDatosANMAT { get; set; }
        public string NroExpedienteEnvio { get; set; }
        public DateTime FechaPresentacionEnvio { get; set; }
        public string NroDisposicionrespuesta { get; set; }
        public DateTime? FechaDisposicionRespuesta { get; set; }
        public string UsuarioRegistros { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public ICollection<Circuitos> Circuitos { get; set; }
        public ICollection<ArchivosDatosANMAT> ArchivosDatosANMAT { get; set; }
    }
}
