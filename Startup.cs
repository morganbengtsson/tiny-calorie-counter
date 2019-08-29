using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using Npgsql;
using Diet.Models;
using Diet.ViewModels;
using Diet.Contexts;
using Diet.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Diet
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(500);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            /*
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("sv"),                
                new CultureInfo("pl"),
            };
            */
            var supportedCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

            services.Configure<RequestLocalizationOptions>(options =>
            {            
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();

            services.AddPortableObjectLocalization(options =>
            options.ResourcesPath = "Localization");

            //TODO: Move to extension method, or more configuratble
            services.AddDbContext<SQLiteContext>();
            var connectionString = new NpgsqlConnectionStringBuilder(Configuration["CloudSql:ConnectionString"])
            {
                SslMode = SslMode.Require,
                TrustServerCertificate = true,
                UseSslStream = true,
            };
            if (string.IsNullOrEmpty(connectionString.Database))
            {
                connectionString.Database = "tiny-calorie-counter";
            }
            NpgsqlConnection connection =
                new NpgsqlConnection(connectionString.ConnectionString);
            connection.ProvideClientCertificatesCallback +=
                certs => certs.Add(new X509Certificate2(
                    Configuration["CloudSql:CertificateFile"]));
            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreContext>(options => options.UseNpgsql(connection));

            if (HostingEnvironment.IsDevelopment())
            {
                services.AddScoped<MainContext>(p => p.GetService<SQLiteContext>());
            }
            else if (HostingEnvironment.IsProduction())
            {
                services.AddScoped<MainContext>(p => p.GetService<PostgreContext>());
            }

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<MainContext>();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddTransient<IEmailSender, EmailSender>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            var options = new RewriteOptions()
                .AddRedirectToHttpsPermanent();
            app.UseRewriter(options);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRequestLocalization();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "diary",
                    template: "{controller=Diary}/{Date:datetime?}/{action=Index}");

            });

            
        }
    }
}
