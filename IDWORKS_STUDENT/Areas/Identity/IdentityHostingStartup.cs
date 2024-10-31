using System;
using IDWORKS_STUDENT.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IDWORKS_STUDENT.Areas.Identity.IdentityHostingStartup))]
namespace IDWORKS_STUDENT.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ApplicationSecurityDBContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ApplicationDBContextConnection")));

              //  services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationSecurityDBContext>().AddDefaultTokenProviders();

               services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationSecurityDBContext>().AddDefaultTokenProviders(); ;
            });
        }
    }
}