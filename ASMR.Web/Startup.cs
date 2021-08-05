//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 5:21 AM
//
// Startup.cs
//
using System.IO;
using System.Security.Cryptography.X509Certificates;
using ASMR.Web.Constants;
using ASMR.Web.Data;
using ASMR.Web.Extensions;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASMR.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.HttpOnly = AntiforgeryConstants.CookieHttpOnly;
                options.Cookie.Name = AntiforgeryConstants.CookieName;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.FormFieldName = AntiforgeryConstants.FormFieldName;
                options.HeaderName = AntiforgeryConstants.HeaderName;
                options.SuppressXFrameOptionsHeader = AntiforgeryConstants.SuppressXFrameOptionsHeader;
            });

            services.AddAuthentication(CookieAuthenticationConstants.AuthenticationScheme)
                .AddCookie(CookieAuthenticationConstants.AuthenticationScheme, options => {
                    options.ClaimsIssuer = CookieAuthenticationConstants.ClaimIssuer;
                    options.Cookie.HttpOnly = CookieAuthenticationConstants.CookieHttpOnly;
                    options.Cookie.Name = CookieAuthenticationConstants.CookieName;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = CookieAuthenticationConstants.ExpireTimeSpan;
                    options.Events.OnRedirectToLogin = CookieAuthenticationConstants.OnRedirectToLogin;
                    options.Events.OnRedirectToAccessDenied = CookieAuthenticationConstants.OnRedirectToAccessDenied;
                });
            services.AddAuthorization();

            services.AddControllersWithViews()
                .AddJsonOptions((options) => {
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        JsonConstants.DefaultJsonSerializerOptions.DefaultIgnoreCondition;
                    options.JsonSerializerOptions.PropertyNamingPolicy =
                        JsonConstants.DefaultJsonSerializerOptions.PropertyNamingPolicy;
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddDataProtection()
                .SetApplicationName(typeof(Startup).Assembly.GetName().Name ?? "ASMR.Web")
                .ProtectKeysWithCertificate(
                    new X509Certificate2(Path.Join("Keys", "DataProtectionCertificate.pfx")))
                .PersistKeysToDbContext<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.EnableSensitiveDataLogging(Environment.IsDevelopment());
                options.UseSqlite(
                    Configuration.GetConnectionString(DatabaseContants.ConnectionStringName), 
                    configure =>
                    {
                        configure.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
            });

            services.AddResponseCaching();
            services.AddResponseCompression();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
                options.LowercaseQueryStrings = true;
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/global.style.css", "styles/global/**/*.css");
                pipeline.AddJavaScriptBundle("/global.script.js", "scripts/global/**/*.js");
            });

            services.Configure<FormOptions>(options => {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            //
            // Custom Services
            //
            // Transient lifetime services are created each time they are requested.
            // This lifetime works best for lightweight, stateless services.
            //
            // Scoped lifetime services are created once per request.
            //
            // Singleton lifetime services are created the first time they are requested
            // (or when ConfigureServices is run if you specify an instance there) and
            // then every subsequent request will use the same instance.
            //
            services.AddScoped<IBeanService, BeanService>();
            services.AddScoped<IMediaFileService, MediaFileService>();
            services.AddScoped<IRoastedBeanProductionService, RoastedBeanProductionService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IHashingService, HashingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSecurityHeaders(SecurityHeadersConstants.DevelopmentHeaderPolicyCollection);
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseSecurityHeaders(SecurityHeadersConstants.DefaultHeaderPolicyCollection);
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            app.UseSelfMigration();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();

            app.UseHttpStatusHandler();

            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAntiforgery();
            app.UseAuthentication();
            app.UseUserAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseApiRouting();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                // if (Environment.IsDevelopment())
                // {
                //     spa.UseProxyToSpaDevelopmentServer(new Uri("http://127.0.0.1:3000/"));
                //     //spa.UseReactDevelopmentServer(npmScript: "start");
                // }
            });
        }
    }
}
