using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrosSanitarios.BO.Models;
using Serilog;
using Serilog.Context;
using RegistrosSanitarios.Servicios;
using RegistrosSanitarios.EntityModel.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using RegistrosSanitarios.EntityModel.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Web.Administration;
using Newtonsoft.Json;
using RegistrosSanitarios.EntityModel.Context;

namespace RegistrosSanitarios.BO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        ServicioUtiles _servUtiles;
        private readonly UserWrapper _userWrapper;
        ServicioUsuarios _servUsuarios;
        private readonly IHostingEnvironment _environment;
        private IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        //private readonly IServicioNovedades _servNovedades;

        public class ControllerBase : Controller
        {

            protected static FileResult FileDownload(string file, string type)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                var response = new FileContentResult(fileBytes, type);
                string fileName = Path.GetFileName(file);
                response.FileDownloadName = fileName;
                return response;
            }

        }

        public HomeController(BDRegistrosSanitariosContext context, UserWrapper userWrapper, IHostingEnvironment env, IConfiguration Configuration, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _logger = Log.ForContext<HomeController>();
            _servUtiles = new ServicioUtiles();
            _userWrapper = userWrapper;
            _environment = env;
            _configuration = Configuration;
            _servUsuarios = new ServicioUsuarios(context, env, httpContextAccessor, Configuration, cache);
            //_servNovedades = sn;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {

            Log.Debug("Autenticando usuario");
            bool simularAutenticacion = Convert.ToBoolean(_configuration["Login:SimularAutenticacion"]);

            if ((_environment.EnvironmentName == "Desa" || _environment.EnvironmentName == "LenovoMMD" || _environment.EnvironmentName == "Test") && Request.Query["user"].Count > 0)
            {
                _userWrapper.UserStub = Request.Query["user"].ToString();

            }
            var isValid = await _servUsuarios.AutenticarUsuarioBO(_userWrapper.GetUser());

            if (!isValid)
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View();
            }
            // Create the identity from the user info
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, _userWrapper.GetUser()));
            identity.AddClaim(new Claim(ClaimTypes.Name, _userWrapper.GetUser()));

            //Verificar que el usuario tenga admitido el acceso a la aplicación
            if (_servUsuarios.PuedeAccederAplicacion(_userWrapper.GetUser()))
                identity.AddClaim(new Claim(ClaimTypes.Role, PermisosEnum.AccesoAplicacion.ToString()));
            else
            {
                ModelState.AddModelError("", "Usuario no autorizado a acceder a la aplicación");
                return View();
            }
            //Verificar que el usuario tenga admitido el acceso al módulo
            if (_servUsuarios.PuedeAccederModulo(_userWrapper.GetUser()))
                identity.AddClaim(new Claim(ClaimTypes.Role, PermisosEnum.AccesoModulo.ToString()));
            else
            {
                ModelState.AddModelError("", "Usuario no autorizado a acceder al módulo");
                return View();
            }


            //Verificar que sea usuario Administrador
            if (_servUsuarios.EsSuperAdmin(_userWrapper.GetUser()))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, PermisosEnum.SuperAdmin.ToString()));
            }
            //Verificar que sea usuario Administrador
            else if (_servUsuarios.EsAdmin(_userWrapper.GetUser()))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, PermisosEnum.Admin.ToString()));
            }
            //Verificar que sea usuario Estudio
            else if (_servUsuarios.EsReportes(_userWrapper.GetUser()))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, PermisosEnum.Reportes.ToString()));
            }


            // Authenticate using the identity
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

  
            if (!string.IsNullOrEmpty(Request.Query["ReturnUrl"]))
            {
                return Redirect(Request.Query["ReturnUrl"]);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        [Authorize(Roles = "AccesoModulo")]
        public IActionResult Index()
        {
            return View("Index");
        }
        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }


        public IActionResult UserInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserInfo(string userName, bool isAdmin, bool reset)
        {
            if (reset)
            {
                _userWrapper.ForceAdmin = false;
                _userWrapper.UserStub = null;
            }
            else
            {
                _userWrapper.ForceAdmin = isAdmin;
                _userWrapper.UserStub = userName;
            }
            return View();
        }

    }
}
