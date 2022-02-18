using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spotifo.Data;
using Spotifo.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotifo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Spotifo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICancionService, CancionService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IGeneroService, GeneroService>();
            services.AddScoped<IFavoritosAlbumService, FavoritosAlbumService>();
            services.AddScoped<IFavoritosCancionService, FavoritosCancionService>();
            services.AddAuthentication("Identity.Application").AddCookie();
            services.AddHttpContextAccessor();

            //Conecction DB
            var SqlConnectionConfiguration = new SqlConnectionConfiguration(Configuration.GetConnectionString("SqlDBContext"));
            //Patr�n Singleton
            services.AddSingleton(SqlConnectionConfiguration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
