using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Z02.Models.DBModel;

namespace Z02
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
            services.AddDistributedMemoryCache();

//            services.AddDbContext<NotesDbContext>(options => options.UseSqlServer("Server=localhost,8200;Database=NTR2019Z;User Id=karbownik;Password=283674;"));

            services.AddSession(options =>
                                {
                                    // Set a short timeout for easy testing.
                                    options.IdleTimeout = TimeSpan.FromSeconds(120);
                                    options.Cookie.HttpOnly = true;
                                });

            services.AddControllersWithViews();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
		endpoints.MapControllerRoute(
                    name: "newOrEditNote",
                    pattern: "{controller=NewOrEditNote}/{action=Index}/{id}");
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Notes}/{action=Index}/{id?}");

            });
        }
    }
}
