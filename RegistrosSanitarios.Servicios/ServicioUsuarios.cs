using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//using Tree.Structures;
using RegistrosSanitarios.EntityModel.Models;
using RegistrosSanitarios.EntityModel.Context;
using RegistrosSanitarios.EntityModel.Enums;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;


namespace RegistrosSanitarios.Servicios
{
    public class ServicioUsuarios
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private List<RelacionHijoPadre> _jerarquia;
        private List<RelacionHijoPadre> _jerarquiaSinPadre;

        private BDRegistrosSanitariosContext _context;
        private readonly object syncLock = new object();
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private enum CategoriaEmpleado {
            Jefe = 14,
            Gerente = 13,
            Directores = 1,
            APM = 8
        }

        public ServicioUsuarios(BDRegistrosSanitariosContext ctx, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, IConfiguration conf , IMemoryCache cache)
        {
            _context = ctx;
            _environment = env;
            _httpContextAccessor = httpContextAccessor;
            _logger = Log.ForContext<ServicioUsuarios>();
            _configuration = conf;
            _cache = cache;
        }

       

        private List<RelacionHijoPadre> ObtenerJerarquia()
        {
            lock (syncLock)
            {
                if (_jerarquia == null)
                {
                    
                    if (!_cache.TryGetValue<List<RelacionHijoPadre>>("Jerarquia", out _jerarquia))
                    {
                        _jerarquia = _context.RelacionHijoPadres.FromSql("dbo.spWKFP_ObtenerJerarquia").ToList();

                        MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                        DateTime today = System.DateTime.Now;
                        DateTime fechaExpiracion = today.AddDays((int)DayOfWeek.Saturday - (int)today.DayOfWeek);

                        cacheExpirationOptions.SetAbsoluteExpiration(fechaExpiracion);
                        cacheExpirationOptions.Priority = CacheItemPriority.High;
                        _cache.Set<List<RelacionHijoPadre>>("Jerarquia", _jerarquia);
                        _logger.Debug("Guardando Jerarquia en cache");
                    }
                    else
                    {
                        _logger.Debug("Obteniendo Jerarquia desde cache");
                    }
                   
                }

                return _jerarquia;
                
            }
        }

        private List<RelacionHijoPadre> ObtenerJerarquia_IncluirSinPadre()
        {
            lock (syncLock)
            {
                if (_jerarquiaSinPadre == null)
                {

                    if (!_cache.TryGetValue<List<RelacionHijoPadre>>("JerarquiaSinPadre", out _jerarquiaSinPadre))
                    {
                        _jerarquiaSinPadre = _context.RelacionHijoPadres.FromSql("dbo.spWKFP_ObtenerJerarquia_IncluirSinPadre").ToList();

                        MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                        DateTime today = System.DateTime.Now;
                        DateTime fechaExpiracion = today.AddDays((int)DayOfWeek.Saturday - (int)today.DayOfWeek);

                        cacheExpirationOptions.SetAbsoluteExpiration(fechaExpiracion);
                        cacheExpirationOptions.Priority = CacheItemPriority.High;
                        _cache.Set<List<RelacionHijoPadre>>("JerarquiaSinPadre", _jerarquiaSinPadre);
                        _logger.Debug("Guardando Jerarquia sin padre en cache");
                    }
                    else
                    {
                        _logger.Debug("Obteniendo Jerarquia sin padre desde cache");
                    }

                }

                return _jerarquiaSinPadre;

            }
        }


        public List<Usuario> TraerHijos(string usuarioAd)
        {
            var jerarquia = ObtenerJerarquia();

            var l = jerarquia.Where(r => r.UsuarioADPadre == usuarioAd)
                .Select(s => new Usuario()
                {
                    Nombre = s.NombreCompletoHijo,
                    UsuarioAD = s.UsuarioADHijo,
                    Email = s.EmailHijo,
                    Categoria = s.CategoriaHijo,
                    IdSector = s.SectorHijo
                }).ToList();

            return l;
        }


        public Usuario ObtenerUsuarioSinPadre(string usuarioAd)
        {
            var jerarquia = ObtenerJerarquia_IncluirSinPadre();
            var u = jerarquia.Where(r => r.UsuarioADHijo == usuarioAd)
                .Select(s => new Usuario()
                {
                    Nombre = s.NombreCompletoHijo,
                    UsuarioAD = s.UsuarioADHijo,
                    Email = s.EmailHijo,
                    Categoria = s.CategoriaHijo,
                    IdSector = s.SectorHijo,
                    Legajo = s.LegajoHijo,
                    Telefono = s.TelefonoHijo
                }).FirstOrDefault();

            return u;
        }
        public Usuario ObtenerUsuario(string usuarioAd)
        {
            var jerarquia = ObtenerJerarquia();
            var u = jerarquia.Where(r => r.UsuarioADHijo == usuarioAd)
                .Select(s => new Usuario()
                {
                    Nombre = s.NombreCompletoHijo,
                    UsuarioAD = s.UsuarioADHijo,
                    Email = s.EmailHijo,
                    Categoria = s.CategoriaHijo,
                    IdSector = s.SectorHijo,
                    Legajo = s.LegajoHijo,
                    Telefono =  s.TelefonoHijo
                }).FirstOrDefault();

            return u;
        }

        public string ObtenerRol(string usuarioAd)
        {
            string rol = "";

            if (EsSuperAdmin(usuarioAd))
                rol = RolesEnum.SUPERADMIN.ToString();
            else if (EsAdmin(usuarioAd))
                rol = RolesEnum.ADMIN.ToString();
            else if (EsReportes(usuarioAd))
                rol = RolesEnum.REPORTES.ToString();

            _logger.Debug("Rol usuario {usuarioAd} {rol}", usuarioAd, rol);
            return rol.ToLower();

        }

        public bool EsJefe(string usuarioAd)
        {
            var u = ObtenerUsuario(usuarioAd);

            return u != null && (u.Categoria == ((int)CategoriaEmpleado.Jefe).ToString());
        }
        public bool EsGerente(string usuarioAd)
        {
            var u = ObtenerUsuario(usuarioAd);

            return u != null && (u.Categoria == ((int)CategoriaEmpleado.Gerente).ToString());
        }
        public bool EsAPM(string usuarioAd)
        {
            var u = ObtenerUsuario(usuarioAd);

            return u != null && (u.Categoria == ((int)CategoriaEmpleado.APM).ToString());
        }

        public bool EsAdmin(string usuarioAd)
        {           
            List<ADGroups> grupos = GruposUsuario();
            Log.Debug("Es admin? {0}", grupos);
            bool simularAutenticacion = Convert.ToBoolean(_configuration["Login:SimularAutenticacion"]);
            if (simularAutenticacion && usuarioAd == "admin")
            {
                return true;
            }
            else
            {
                if (grupos != null)
                {
                    Log.Debug("grupo admin {0} ", grupos.Any(g => g.Name == PermisosEnum.WEB_RegistrosSanitarios_Configuracion.ToString()));
                    return grupos.Any(g => g.Name == PermisosEnum.WEB_RegistrosSanitarios_Configuracion.ToString());
                }
                return false;
            }
        }
        public bool EsSuperAdmin(string usuarioAd)
        {
            List<ADGroups> grupos = GruposUsuario();
            Log.Debug("Es super admin? {0}", grupos);

            bool simularAutenticacion = Convert.ToBoolean(_configuration["Login:SimularAutenticacion"]);
            if (simularAutenticacion && usuarioAd == "superadmin")
            {
                return true;
            }
            else
            {
                if (grupos != null)
                {
                    Log.Debug("grupo super admin {0} ", grupos.Any(g => g.Name == PermisosEnum.WEB_RegistrosSanitarios_Configuracion.ToString()));
                    return grupos.Any(g => g.Name == PermisosEnum.WEB_RegistrosSanitarios_Configuracion.ToString());
                }
                return false;
            }
        }
        public bool EsReportes(string usuarioAd)
        {
            List<ADGroups> grupos = GruposUsuario();
            Log.Debug("Es reporte? {0}", grupos);

            if (grupos != null)
            {
                Log.Debug("grupo reportes {0} ", grupos.Any(g => g.Name == PermisosEnum.WEB_RegistrosSanitarios_Reportes.ToString()));
                return grupos.Any(g => g.Name == PermisosEnum.WEB_RegistrosSanitarios_Reportes.ToString());
            }
            return false;
        }

        public List<Funciones> FuncionesUsuario()
        {
            //si no se encuentran funciones es porque recicló el iis, volver a autenticar
            if (_session.Get<List<ADGroups>>("FuncionesUsuario") == null)
                _httpContextAccessor.HttpContext.Response.Redirect("/Home/Login");

            return _session.Get<List<Funciones>>("FuncionesUsuario");
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }

        public List<ADGroups> GruposUsuario()
        {
            //si no se encuentran grupos es porque recicló el iis, volver a autenticar
            if (_session.Get<List<ADGroups>>("GruposUsuario") == null)
            {
                string baseUrl = GetBaseUrl();
                string urlRedirect = System.Net.WebUtility.UrlEncode(baseUrl + _httpContextAccessor.HttpContext.Request.Path.Value.ToString());
                string urlLogin = baseUrl + "/Home/Login?ReturnUrl=";
                _httpContextAccessor.HttpContext.Response.Redirect(urlLogin + urlRedirect);
            }
               
            return _session.Get<List<ADGroups>>("GruposUsuario");
        }

        private void SimularPermisosUsuario(string userName, ref List<ADGroups> grupos, ref List<Funciones> funciones)
        {
            string[] gruposReq = null;

            if (userName.Length >= 8 && userName.ToUpper().Substring(0,8) =="REPORTES")
                gruposReq = _configuration["Login:GruposReportes"].Split(",");
            else if (userName.Length >= 5 && userName.ToUpper().Substring(0, 5) == "ADMIN")
                gruposReq = _configuration["Login:GruposAdmin"].Split(",");
            else if (userName.Length >= 10 && userName.ToUpper().Substring(0, 10) == "SUPERADMIN")
                gruposReq = _configuration["Login:GruposSuperAdmin"].Split(",");
            else
                gruposReq = null;
            
            if (gruposReq != null)
            { 
                foreach (string grupo in gruposReq)
                {
                    grupos.Add(new ADGroups { Name = grupo });
                    switch (grupo)
                    {
                        case "WEB_RegistrosSanitarios_Configuracion":
                            funciones.Add(new Funciones { IdPermiso = 0, GrupoAD = grupo, Seccion = "Configuracion", Tipo = "SECCION" });
                            break;
                        case "WEB_RegistrosSanitarios_Reportes":
                            funciones.Add(new Funciones { IdPermiso = 0, GrupoAD = grupo, Seccion = "Reportes", Tipo = "SECCION" });
                            break;
                        case "WEB_RegistrosSanitarios_APP":
                            funciones.Add(new Funciones { IdPermiso = 24, GrupoAD = grupo, Seccion = "", Tipo = "APLICACION" });
                            funciones.Add(new Funciones { IdPermiso = 59, GrupoAD = grupo, Seccion = "", Tipo = "MODULO" });
                            break;

                    }
                }
            }            

        }

        public async Task<bool> AutenticarUsuarioBO(string userNameWithDomain)
        {
            List<ADGroups> grupos = new List<ADGroups>();
            List<Funciones> funcionesSeguridad = new List<Funciones>();
            Funciones f = new Funciones();
            var stringResult = "";
            var response = new HttpResponseMessage();
            string idApp = _configuration["Login:IdApp"];
            string idModulo = _configuration["Login:IdModulo"];
            
            int posUsuario = userNameWithDomain.IndexOf("\\");
            string userName = "";
            if (posUsuario > -1)
                userName = userNameWithDomain.Substring(posUsuario + 1);
            else
                userName = userNameWithDomain;

            using (var client = new HttpClient())
            {
                try
                {
                    //Validar las credenciarles del usuario contra AD, y obtener los grupos de seguridad habilitados
                    string apiSeguridad = _configuration["Login:APISeguridad"];
                    bool simularAutenticacion = Convert.ToBoolean(_configuration["Login:SimularAutenticacion"]);
                    string ouAplicacion = _configuration["Login:ouAplicacion"];
                    if (simularAutenticacion)
                    {
                        _logger.Debug("SIMULANDO AUTENTICACION BO {userName}", userName);
                        //Por si no funciona la Web Api de Seguridad, para que se puedan hacer pruebas.
                        SimularPermisosUsuario(userName, ref grupos, ref funcionesSeguridad);
                    }
                    else
                    {
                        _logger.Debug("AUTENTICACION REAL BO {userName}", userName);
                        client.BaseAddress = new Uri(apiSeguridad);
                        //response = await client.GetAsync($"/apiAD/Usuarios/?userName={userName}&ouAplicacion={ouAplicacion}");
                        response = await client.GetAsync($"apiAD/Usuarios/{userName}/{ouAplicacion}");
                        response.EnsureSuccessStatusCode();

                        stringResult = await response.Content.ReadAsStringAsync();
                        grupos = JsonConvert.DeserializeObject<List<ADGroups>>(stringResult);

                        //Obtener los permisos establecidos para la aplicación en la base de Seguridad                   
                        response = null;
                        stringResult = "";
                        response = await client.GetAsync($"apiAD/Aplicaciones/FuncionesUsuario/{idApp}/{idModulo}/{userName}");
                        response.EnsureSuccessStatusCode();

                        stringResult = await response.Content.ReadAsStringAsync();
                        funcionesSeguridad = JsonConvert.DeserializeObject<List<Funciones>>(stringResult);
                    }

                    _session.Set<List<ADGroups>>("GruposUsuario", grupos);
                    _session.Set<List<Funciones>>("FuncionesUsuario", funcionesSeguridad);

                    await ObtenerAdministradores();

                    return true;
                }
                catch (HttpRequestException httpRequestException)
                {
                    _logger.Here().Error(httpRequestException.Message);
                    return false;
                }
            }
        }
        public async Task ObtenerAdministradores()
        {

            List<string> usuarios = new List<string>();
            var stringResult = "";
            var response = new HttpResponseMessage();

            using (var client = new HttpClient())
            {
                try
                {
                    //Validar las credenciales del usuario contra AD, y obtener los grupos de seguridad habilitados
                    string apiSeguridad = _configuration["Login:APISeguridad"];
                    string grupoAdministrador = "WEB_RegistrosSanitarios_Configuracion";//creo que falta un ad que sea WEB_RegistrosSanitarios_Administrador
                    string ouAplicacion = _configuration["Login:ouAplicacion"];
                    client.BaseAddress = new Uri(apiSeguridad);

                    response = await client.GetAsync($"apiAD/Aplicaciones/AdministradoresAplicacion/{ouAplicacion}/{grupoAdministrador}");
                    response.EnsureSuccessStatusCode();

                    stringResult = await response.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<string>>(stringResult);


                    _session.Set<List<string>>("Administradores", usuarios);
                }
                catch (HttpRequestException httpRequestException)
                {
                    _logger.Here().Error(httpRequestException.Message);

                }
            }
        }


        /* acceso a la aplicación */
        public bool PuedeAccederAplicacion(string usuarioAd)
        {
            return (FuncionesUsuario().Any(f => f.Tipo == TiposPermisosEnum.APLICACION.ToString()));            
        }
        /* acceso al módulo */
        public bool PuedeAccederModulo(string usuarioAd)
        {
            return (FuncionesUsuario().Any(f => f.Tipo == TiposPermisosEnum.MODULO.ToString()));
        }
        /* acceso a sección */
        public bool PuedeAccederPagina(string usuarioAd, string nombrePagina)
        {
            return (FuncionesUsuario().Any(f => f.Tipo == TiposPermisosEnum.SECCION.ToString() && f.Seccion == nombrePagina ));            
        }
        
        
    }

}