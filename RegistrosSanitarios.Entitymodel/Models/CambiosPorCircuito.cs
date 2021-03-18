using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class CambiosPorCircuito
    {
        public CambiosPorCircuito() { }
        public int IdCambiosPorCircuito { get; set; }
        public int IdCircuitos { get; set; }
        public int? IdProductosRegistrosSanit { get; set; }
        public int? IdIndicacionesProductos { get; set; }
        public int? IdexcipientesProductos { get; set; }
        public int? IdConcentraciones { get; set; }
        public string NombreCampo { get; set; }
        public string ValorActual { get; set; }
        public string ValorActualizado { get; set; }
        public int IdEstadosCambiosPorCircuito { get; set; }
        public DateTime FechaSolicitudCambio { get; set; }
        public DateTime? FechaAplicacionCambio { get; set; }
        public EstadosCambiosPorCircuito IdEstadosCambiosPorCircuitoNavigation { get; set; }
        public Circuitos IdCircuitosNavigation { get; set; }
    }
}
