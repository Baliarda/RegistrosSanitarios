using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class Circuitos
    {
        public Circuitos()
        {
            CambiosPorCircuito = new HashSet<CambiosPorCircuito>();
            TareasProductosRegistrosSanit = new HashSet<TareasProductosRegistrosSanit>();
            TareasRegistrosSanitarios = new HashSet<TareasRegistrosSanitarios>();
        }
        public int IdCircuitos { get; set; }
        public int IdRegistrosSanitarios { get; set; }
        public int IdDatosANMAT { get; set; }
        public int IdtiposCircuitos { get; set; }
        public DateTime FechaInicio { get; set; }
        public string UsuarioInicio { get; set; }
        public int IdestadosCircuitos { get; set; }
        public DatosANMAT IdDatosANMATNavigation { get; set; }
        public EstadosCircuitos IdestadosCircuitosNavigation { get; set; }
        public TiposCircuitos IdtiposCircuitosNavigation { get; set; }
        public ICollection<CambiosPorCircuito> CambiosPorCircuito { get; set; }
        public ICollection<TareasProductosRegistrosSanit> TareasProductosRegistrosSanit { get; set; }
        public ICollection<TareasRegistrosSanitarios> TareasRegistrosSanitarios { get; set; }
    }
}

