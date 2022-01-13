using EmptyPlatform.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace EmptyPlatform.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _conf;

        public Startup(IWebHostEnvironment env, IConfiguration conf)
        {
            _env = env;
            _conf = conf;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllers(configure =>
            {
                configure.Filters.Add<AuthorizationFilter>();
                configure.Filters.Add<ValidationFilter>();
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    /*
Content-Security-Policy – ограничение доверенных доменов для предотвращения возможных XSS атак
Заголовок X-Frame-Options для защиты от атак типа clickjacking.
X-XSS-Protection – принудительно включить встроенный механизм защиты браузера от XSS атак.
X-Content-Type-Options – для защиты от подмены MIME типов.
                     */
                    options.Cookie.HttpOnly = true;
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.IsEssential = true;
                    options.ExpireTimeSpan = new TimeSpan(0, 10, 0);
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = (context) =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                        return Task.CompletedTask;
                    };
                });
            services.AddSwaggerGen(options =>
            {
                // TODO: schema filter by permission
                // TODO: add display descriptions
                options.DocumentFilter<SwaggerDocumentFilter>();
                options.CustomSchemaIds(type => type.FullName);
            });
            services.AddAuth();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IDbConnection>((IServiceProvider sp) => new SqliteConnection(_conf.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Get}/{id?}");
            });
        }
    }
}
