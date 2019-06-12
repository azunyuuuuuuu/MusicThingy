using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using EFCore.DbContextFactory.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MusicThingy.Models;
using MusicThingy.Services;
using YoutubeExplode;
using ElectronNET.API.Entities;

namespace MusicThingy
{
    public class Startup
    {
        private readonly Configuration _config;

        public Startup(IConfiguration config)
        {
            _config = config.GetSection("config").Get<Models.Configuration>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // services.AddDbContext<AppDbContext>();
            services.AddDbContextFactory<AppDbContext>(builder => builder
                .UseSqlite("Data Source=" + Path.Combine(_config.DataPath, "library.sqlite")));
            services.AddScoped<DataRepository>();

            services.AddHttpClient();
            services.AddScoped<YoutubeClient>();

            services.AddScoped<SyncService>();

            services.AddHostedService<YouTubeFetchingService>();
            services.AddHostedService<YouTubeDownloadService>();
            services.AddHostedService<TagUpdateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            Directory.CreateDirectory(Path.Combine(_config.DataPath, "sources"));
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(_config.DataPath, "sources")),
                RequestPath = "/sources"
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions { });
        }
    }
}
