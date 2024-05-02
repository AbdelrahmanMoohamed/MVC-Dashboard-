using Demo.BLL;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Services;
using Demo.PL.Settings;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // Register Built in MVC Services to The Container

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });


            //services.AddScoped();         // LifeTime Per Request - Object UnReachable
            //services.AddTransient();      // LifeTime Per Opeatrion
            //services.AddSingleton();      // LifeTime Per App

            services.AddScoped<IScopedService, ScopedService>();          // LifeTime Per Request 
            services.AddTransient<ITransientService, TransientService>(); // LifeTime Per Opeatrion
            services.AddSingleton<ISingeltonService, SingeltonService>(); // LifeTime Per App

            services.AddAutoMapper(M => M.AddProfile(new MapProfile()));

            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();


            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(congfig =>
            {
                congfig.LoginPath = "/Account/SignIn";
            });

            // MailKit
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSettings, EmailSettings>();
            // Twilio
            services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));

			services.AddTransient<ISmsService, SmsService>();
            // External Login with Google
            services.AddAuthentication(o =>
           {
               o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
               o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
           }).AddGoogle(o => {
               IConfiguration GoogleAuthentcation = Configuration.GetSection("Authentication:Google");
               o.ClientId = GoogleAuthentcation["ClientId"];
               o.ClientSecret = GoogleAuthentcation["ClientSecret"];
           });



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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
