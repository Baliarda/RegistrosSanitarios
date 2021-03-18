using System;
using System.Collections.Generic;

namespace RegistrosSanitarios.EntityModel.Models
{
    public partial class ProductosRegistrosSanit
    {
        public ProductosRegistrosSanit()
        {
            ArchivosProductosRegistrosSanit = new HashSet<ArchivosProductosRegistrosSanit>();
            ExcipientesProductos = new HashSet<ExcipientesProductos>();
            IndicacionesProductos = new HashSet<IndicacionesProductos>();
            TareasProductosRegistrosSanit = new HashSet<TareasProductosRegistrosSanit>();
        }
        public int IdProductosRegistrosSanit { get; set; }
        public int IdRegistrosSanitarios { get; set; }
        public int IdFormasFarmaceuticas { get; set; }
        public int IdPresentaciones { get; set; }
        public int IdConcentraciones { get; set; }
        public int IdEnvasesPrimarios { get; set; }
        public int IdCondicionesConservacion { get; set; }
        public string CompCualicuantitativaExcipiente { get; set; }
        public int? NroTroquelN { get; set; }
        public string VidaUtil { get; set; }
        public string ContenidoEnvasePrimario { get; set; }
        public string ContenidoEnvaseSecundario { get; set; }
        public string CondicionConservacionFormaReconstituida { get; set; }
        public string SitioElaboracion { get; set; }
        public string SitioAcondicionamientoPrimario { get; set; }
        public string SitioAcondicionamientoSecundario { get; set; }
        public int? IdTareaActual { get; set; }
        public int IdEstadosProductosRegistrosSanit { get; set; }
        public bool? Estado { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public Concentraciones IdConcentracionesNavigation { get; set; }
        public CondicionesConservacion IdCondicionesConservacionNavigation { get; set; }
        public EnvasesPrimarios IdEnvasesPrimariosNavigation { get; set; }
        public EstadosProductosRegistrosSanit IdEstadosProductosRegistrosSanitNavigation { get; set; }
        public FormasFarmaceuticas IdFormasFarmaceuticasNavigation { get; set; }
        public Presentaciones IdPresentacionesNavigation { get; set; }
        public ICollection<ArchivosProductosRegistrosSanit> ArchivosProductosRegistrosSanit { get; set; }
        public ICollection<ExcipientesProductos> ExcipientesProductos { get; set; }
        public ICollection<IndicacionesProductos> IndicacionesProductos { get; set; }
        public ICollection<TareasProductosRegistrosSanit> TareasProductosRegistrosSanit { get; set; }
    }
}
