using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RegistrosSanitarios.EntityModel.Context;

namespace RegistrosSanitarios.BO
{
    public class DataInitializer
    {
        private BDRegistrosSanitariosContext _ctx;
        public DataInitializer(BDRegistrosSanitariosContext ctx)
        {
            _ctx = ctx;
        }

        public void CleanUp()
        { 
           _ctx.ChangeTracker.Entries()
               .Where(e => e.Entity != null).ToList()
               .ForEach(e => e.State = EntityState.Detached );
        }
        
        
    }
}
