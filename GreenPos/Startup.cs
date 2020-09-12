using AutoMapper;
using GreenPos.Model;
using GreenPOS.Common;
using GreenPOS.Context;
using GreenPOS.Interfaces;
using GreenPOS.Mapper;
using GreenPOS.Repository;
using GreenPOS.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GreenPOS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllersWithViews();

            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            // ######### AutoMapper Configuration ################
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest); ;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CommonMappingProfile>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            // ######### AutoMapper Configuration ################

            services.AddDbContext<GreenPOSDBContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("CS_ConnectionString")));

            //configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSession();

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddSingleton<IDataCache, DataCache>();
            services.AddScoped<IAzureStorageService, AzureStorageService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();


            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSession();

            app.Use(async (context, next) =>
            {
                var bToken = context.Session.GetString(Constants.Token.ToString());
                if (!string.IsNullOrEmpty(bToken))
                    context.Request.Headers.Add("Authorization", "Bearer " + bToken);

                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                if (!serviceScope.ServiceProvider.GetService<GreenPOSDBContext>().AllMigrationsApplied())
                    serviceScope.ServiceProvider.GetService<GreenPOSDBContext>().Database.Migrate();

                serviceScope.ServiceProvider.GetService<GreenPOSDBContext>().EnsureSeeded();
            }


        }
    }
}
