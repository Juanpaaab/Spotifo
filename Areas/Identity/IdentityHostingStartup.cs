using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifo.Data;
using Spotifo.Areas.Identity.Data;

[assembly: HostingStartup(typeof(Spotifo.Areas.Identity.IdentityHostingStartup))]
namespace Spotifo.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SpotifoContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SpotifoContextConnection")));

                services.AddDefaultIdentity<SpotifoUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SpotifoContext>();
            });
        }
    }
}