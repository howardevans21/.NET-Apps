using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IDWORKS_STUDENT.Models;
using IDWORKS_STUDENT.Repository;
using System.Reflection;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using IDWORKS_STUDENT.Data;
using Microsoft.Extensions.Logging;
using IDWORKS_STUDENT.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IDWORKS_STUDENT
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
            // Configure IIS
            /*
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            }); */
           

            services.AddHttpContextAccessor();

           
            services.AddControllersWithViews();
        
            services.AddRazorPages();
            services.AddScoped<IFileUploadService, LocalFileUploadService>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            var builder = services.AddIdentityCore<IdentityUser>();
            builder.AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationSecurityDBContext>();

            /***********************************************
            * Email Configuration and Inactivity Timeout
            ************************************************/
            builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();
            builder.Services.Configure<AuthMessageSenderOptions>(Configuration);

            var sendGridAPIKey = Configuration.GetConnectionString("EnvironmentVariablesConfigurationProvider");
     
            builder.Services.ConfigureApplicationCookie(o => {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });
            builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));
            /************************************************
             * Roles Context
             **********************************************/


            // Windows Authenticated Security for IIS
            /*
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddAuthorization(); */

            // This can cause pages to be directed to Account\Login Folder 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "";
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        options.SlidingExpiration = true;
                        options.AccessDeniedPath = "/Forbidden"; });

            services.AddAuthorization();
            services.AddDbContext<StudentContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
           
            /*
            services.AddDbContext<StudentContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                options => options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))); */


            //  services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //.AddEntityFrameworkStores<LibDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            setupConfigurations();
            /******************************************
             * Roles 
             *************************************************/

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationSecurityDBContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                ContextSeed.SeedRolesAsync(userManager, roleManager).Wait();
                ContextSeed.SeedSuperAdminAsync(userManager, roleManager).Wait();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }

            app.UseAuthentication();
      

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.Environment = "Test";
            }
            else
            {
                IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.Environment = "Production";
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

         

           // app.UseHttpsRedirection();
            app.UseStaticFiles();
      

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"); 
                endpoints.MapRazorPages();
            

               
            });

         


            // Windows Authenticated Security for Web App
            //  app.UseAuthentication();


            serviceProvider.GetService<StudentContext>().Database.Migrate();
            serviceProvider.GetService<IDWORKS_STUDENT.Data.ApplicationSecurityDBContext>().Database.Migrate();

          
        }

        private void setupConfigurations()
        {
            var appConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string coverageColumnNames = appConfig.GetValue<string>("AppSettings:CoverageColumnNames");
            string firstNameColumnNames = appConfig.GetValue<string>("AppSettings:FirstNameColumnNames");
            string lastNameColumnNames = appConfig.GetValue<string>("AppSettings:LastNameColumnNames");
            string schoolColumnNames = appConfig.GetValue<string>("AppSettings:SchoolColumnNames");
            string policyColumnNames = appConfig.GetValue<string>("AppSettings:PolicyColumnNames");
            string effectiveDateColumnNames = appConfig.GetValue<string>("AppSettings:EffectiveDateColumnNames");
            string coverageProviderColumnNames = appConfig.GetValue<string>("AppSettings:CoverageProviderColumnNames");
            int numberOfColumns = appConfig.GetValue<int>("AppSettings:NumberofColumns");
            string sendGridAPI_Key = appConfig.GetValue<string>("AppSettings:SendGridApiKey");
            string sendGrid_Sender = appConfig.GetValue<string>("AppSettings:SendGridSender");
            string defaultSuperAdmin = appConfig.GetValue<string>("AppSettings:DefaultSuperAdmin");
            string defaultSuperAdminPassword = appConfig.GetValue<string>("AppSettings:DefaultSuperAdminPassword");

        
            IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.DefaultSuperAdmin = defaultSuperAdmin;
            IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.DefaultSuperAdminPassword = defaultSuperAdminPassword;

            string[] s = coverageColumnNames.Split(',', ';');
            foreach(string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._coverageColumns.Add(col); }

            s = coverageProviderColumnNames.Split(',', ';');
            foreach (string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._coverageProviderColumns.Add(col); }

            s = firstNameColumnNames.Split(',', ';');
            foreach (string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._firstNameColumns.Add(col); }

            s = lastNameColumnNames.Split(',', ';');
            foreach (string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._lastNameColumns.Add(col); }

            s = schoolColumnNames.Split(',', ';');
            foreach (string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._schoolColumns.Add(col); }

            s = policyColumnNames.Split(',', ';');
            foreach (string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._policyColumns.Add(col); }

            s = effectiveDateColumnNames.Split(',', ';');
            foreach (string col in s) { IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._effectiveDateColumns.Add(col); }

            IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._numberOfColumns = numberOfColumns;
        }
    }
}
