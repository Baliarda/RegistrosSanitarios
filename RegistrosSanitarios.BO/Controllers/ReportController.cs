using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.Extensions.Configuration;
using RegistrosSanitarios.Servicios;
using System.IO;

namespace RegistrosSanitarios.BO.Controllers
{
    [Authorize(Roles = "AccesoModulo")]
    public class ReportController : AlanJuden.MvcReportViewer.ReportController
    {
        private IConfiguration _configuration;
        private readonly UserWrapper _userWrapper;
        ServicioUsuarios _servUsuarios;
        public ReportController(IConfiguration configuration, UserWrapper userWrapper, ServicioUsuarios servUsuarios)
        {
            _userWrapper = userWrapper;
            _configuration = configuration;
            _servUsuarios = servUsuarios;
        }
       
        protected override ICredentials NetworkCredentials
        {
            get
            {
                var user = _configuration["Reportes:User"];
                var pwd = _configuration["Reportes:Pwd"];
                var domain = _configuration["Reportes:Domain"];
                var useDefaultCredentials = _configuration["Reportes:useDefaultCredentials"];
                if (useDefaultCredentials == "false")
                {
                    //Custom Domain authentication (be sure to pull the info from a config file)
                    return new System.Net.NetworkCredential(user, pwd, domain);
                }
                else
                {
                    //Default domain credentials (windows authentication)
                    return System.Net.CredentialCache.DefaultNetworkCredentials;
                }
            }
        }


        protected override string ReportServerUrl
        {
            get
            {
                //You don't want to put the full API path here, just the path to the report server's ReportServer directory that it creates (you should be able to access this path from your browser: https://YourReportServerUrl.com/ReportServer/ReportExecution2005.asmx )
                
                var reportingServer = _configuration["Reportes:Server"];
                return reportingServer;
            }
        }

        //Only override this property if your controller is not called ReportController.
        //You'll want to enter whatever your controller's name is in place of "YourController" below.
        protected override string ReportImagePath
        {
            get
            {
                //This is the default as an example
                //return "/Report/ReportImage/?originalPath={0}";

                return "/Report/ReportImage/?originalPath={0}";
            }
        }


        [Authorize(Roles = "Admin,SuperAdmin,Reportes")]
        public ActionResult ErroresValores()
        {
            ViewBag.SideBarActiveItem = "ErroresValores";
            var model = this.GetReportViewerModel(Request);
            var carpetaReporting = _configuration["Reportes:CarpetaReporting"];
            model.ReportPath = carpetaReporting + "rptErrorLamadaApi";


            @ViewBag.Titulo = "Reporte Valores con Errores";
            @ViewBag.Reporte = "Monitoreo Errores";
            return View("ReportViewer", model);
        }

        [Authorize(Roles = "Admin,SuperAdmin,Reportes")]
        public ActionResult RegistrosSanitariosDetalles()
        {
            ViewBag.SideBarActiveItem = "RegistrosSanitariosDetalles";

            var model = this.GetReportViewerModel(Request);
            var carpetaReporting = _configuration["Reportes:CarpetaReporting"];
            model.ReportPath = carpetaReporting + "rptSensorHumedadTemperatura";


            @ViewBag.Titulo = "Reporte RegistrosSanitarios UMA";
            @ViewBag.Reporte = "Monitoreo RegistrosSanitarios";
            return View("ReportViewer", model);
        }


    }
}