using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTemplate2.AppLib.Ext.Startup
{
    public static class StatusCodePages
    {
        public static IServiceCollection _AddSCP(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder _UseSCP(this IApplicationBuilder app)
        {
            app.UseStatusCodePagesWithReExecute("/Account/SCP/{0}");
            return app;
        }
    }
}
