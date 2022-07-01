using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Practica.Infraestructure.BrokerService;
using Practica.Infraestructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Application.Boopstrap
{
    public static class ServicesConfiguration
    {
        public static void AddInfraestructueServices(this IServiceCollection services, Action<InfraOptions> infraOptions)
        {
            services.PostConfigure(infraOptions);
            
            services.AddDbContext<PedidosContext>(
            (sp, options) => options.UseSqlServer(
                sp.GetRequiredService<IOptions<InfraOptions>>().Value.ConnectionString,
                x => x.MigrationsAssembly("Practica.API")), ServiceLifetime.Singleton);

            services.AddSingleton<IAMQPublisher, AMQPublisher>();
            services.AddSingleton<IDataAccess, DataAccess>();
        }

        public class InfraOptions
        {
            public string ConnectionString { get; set; }
        }
    }
}
