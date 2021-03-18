using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;

namespace RegistrosSanitarios.BO
{
    public class UserWrapper
    {
        private readonly IHttpContextAccessor _context;

        public string UserStub { get; set; }
        public bool ForceAdmin { get; set; }
        private readonly IConfiguration _configuration;

        public UserWrapper(IHttpContextAccessor context, IConfiguration conf)
        {
            _context = context;
            UserStub = null;
            ForceAdmin = false;
            _configuration = conf;
        }

        public string GetUser()
        {
            bool simularAutenticacion = Convert.ToBoolean(_configuration["Login:SimularAutenticacion"]);
            if (simularAutenticacion)
                return UserStub ?? "admin";
            else
                return UserStub ?? _context.HttpContext.User?.Identity?.Name;
        }

        public bool IsAdmin()
        {
            return ForceAdmin;
            
            //TODO: Aca tiene que ir la llamada para determinar si el usuario es admin o no
        }

        public string GetBuildVersion()
        {
            return "";

        }

    }
}