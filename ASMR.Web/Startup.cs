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
using System.Text;
using ASMR.Common.Constants;
using ASMR.Core.Entities;
using ASMR.Web.Configurations;
using ASMR.Web.Constants;
using ASMR.Web.Data;
using ASMR.Web.Extensions;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ASMR.Web;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	private IConfiguration Configuration { get; }

	//
	// This method gets called by the runtime. Use this method to add services to the container.
	//
	public void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
	{
		//
		// Add options
		//
		services.AddOptions<CaptchaOptions>().BindConfiguration("Google:reCAPTCHA");
		services.AddOptions<JsonWebTokenOptions>().BindConfiguration("JsonWebToken");
		services.AddOptions<MailOptions>().BindConfiguration("SendGrid");

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
		services.AddScoped<IPasswordHasher<User>, Argon2PasswordHashingService>();
		services.AddScoped<ICaptchaService, CaptchaService>();
		services.AddScoped<IEmailService, EmailService>();
		services.AddScoped<IBusinessAnalyticService, BusinessAnalyticService>();
		services.AddScoped<IBeanService, BeanService>();
		services.AddScoped<IMediaFileService, MediaFileService>();
		services.AddScoped<IRoastingService, RoastingService>();
		services.AddScoped<IPackagingService, PackagingService>();
		services.AddScoped<IProductService, ProductService>();
		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IUserService, UserService>();


		//
		// Hosted service
		//
		services.AddHostedService<AutomaticRoastingCancellationService>();

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

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = Configuration["JsonWebToken:Issuer"],
				ValidAudience = Configuration["JsonWebToken:Issuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JsonWebToken:Key"]))
			};
		});

		services.AddControllersWithViews(options =>
			{
				options.CacheProfiles.Add("MediaFileCache", new CacheProfile()
				{
					Duration = 60 * 60 * 24 * 365,
					Location = ResponseCacheLocation.Any
				});
			})
			.AddJsonOptions(options =>
			{
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

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.EnableSensitiveDataLogging(environment.IsDevelopment());
			options.UseSqlite(
				Configuration.GetConnectionString(DatabaseConstants.ConnectionStringName),
				configure => { configure.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
		});

		services.AddIdentity<User, UserRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters =
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.AddDataProtection()
			.SetApplicationName(typeof(Startup).Assembly.GetName().Name ?? "asmr")
			.ProtectKeysWithCertificate(new X509Certificate2(
				Path.Join("Keys", Configuration["DataProtection:Certificate:FileName"]),
				Configuration["DataProtection:Certificate:Password"]))
			.PersistKeysToDbContext<ApplicationDbContext>();

		services.AddResponseCaching();
		services.AddResponseCompression();

		services.AddRouting(options =>
		{
			options.LowercaseUrls = true;
			options.AppendTrailingSlash = false;
			options.LowercaseQueryStrings = true;
		});

		//
		// In production, the React files will be served from this directory
		//
		services.AddSpaStaticFiles(configuration => { configuration.RootPath = ClientAppConstants.BuildPath; });

		services.Configure<FormOptions>(options =>
		{
			options.ValueLengthLimit = int.MaxValue;
			options.MultipartBodyLengthLimit = int.MaxValue;
			options.MemoryBufferThreshold = int.MaxValue;
		});

		services.ConfigureApplicationCookie(options =>
		{
			options.ClaimsIssuer = CookieAuthenticationConstants.ClaimIssuer;
			options.Cookie.HttpOnly = CookieAuthenticationConstants.CookieHttpOnly;
			options.Cookie.Name = AuthenticationConstants.CookieName;
			options.Cookie.SameSite = SameSiteMode.Strict;
			options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			options.ExpireTimeSpan = CookieAuthenticationConstants.ExpireTimeSpan;
			options.Events.OnRedirectToLogin = CookieAuthenticationConstants.OnRedirectToLogin;
			options.Events.OnRedirectToAccessDenied = CookieAuthenticationConstants.OnRedirectToAccessDenied;
		});
	}

	//
	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	//
	public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
	{
		if (!environment.IsDevelopment())
		{
			app.UseExceptionHandler("/error");
			app.UseSecurityHeaders(SecurityHeadersConstants.DefaultHeaderPolicyCollection);

			//
			// The default HSTS value is 30 days. You may want to change this for production
			// scenarios, see https://aka.ms/aspnetcore-hsts.
			//
			app.UseHsts();
		}
		else
		{
			app.UseSecurityHeaders(SecurityHeadersConstants.DevelopmentHeaderPolicyCollection);
		}

		app.UseSerilogRequestLogging();

		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
		});
		app.UseHttpsRedirection();
		app.UseHttpStatusHandler();

		app.UseStaticFiles();
		app.UseSpaStaticFiles();

		app.UseRouting();
		app.UseCors();
		app.UseResponseCaching();
		app.UseResponseCompression();

		app.UseClientPlatformVerification();
		app.UseAntiforgery();
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller}/{action=Index}/{id?}");
		});
		app.UseApiRouting();

		app.UseClientApp(environment);
	}
}