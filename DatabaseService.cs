using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using Npgsql;
using System.Security.Cryptography.X509Certificates;
using Diet.Contexts;

namespace ConfigureExtensions
{

    public static class ProductionExtension
    {
        public static IServiceCollection UseProductionDatabase(this IServiceCollection services,
        IHostingEnvironment env,
        IConfigurationRoot config)
        {
            var connectionString = new NpgsqlConnectionStringBuilder(config["CloudSql:ConnectionString"])
            {
                SslMode = SslMode.Require,
                TrustServerCertificate = true,
                UseSslStream = true,
            };
            if (string.IsNullOrEmpty(connectionString.Database))
            {
                connectionString.Database = "diet";
            }
            NpgsqlConnection connection =
                new NpgsqlConnection(connectionString.ConnectionString);
            connection.ProvideClientCertificatesCallback +=
                certs => certs.Add(new X509Certificate2(
                    config["CloudSql:CertificateFile"]));

            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreContext>(options => options.UseNpgsql(connection));
            services.AddScoped<MainContext>(p => p.GetService<PostgreContext>());

            return services;
        }
    }

    public static class DevelopmentExtension
    {
        public static IServiceCollection UseDevelopmentDatabase(this IServiceCollection services,
        IHostingEnvironment env,
        IConfigurationRoot config)
        {
            services.AddDbContext<SQLiteContext>();
            services.AddScoped<MainContext>(p => p.GetService<SQLiteContext>());
            return services;
        }
    }
}
