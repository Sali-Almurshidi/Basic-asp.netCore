using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // to customize Error message 
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
                // options.Password.RequireNonAlphanumeric = false;
            });

            var connectionString = _config.GetConnectionString("DevConnections");

            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMvc().AddXmlDataContractSerializerFormatters();
            //services.AddMvc();
            // for local array
            //services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            // for local db
            // AddScoped the instance for repo class alive and avalible
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();


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
                // to display page not found error
                //app.UseStatusCodePages();

                //to customize th not found error page 
                // take error 404 and status 200 ok 
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // tow way to customize not found error page 
                // take just status 404 
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("foo.html");

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");

            // default files has be before usestaticfiles
            //app.UseDefaultFiles(defaultFilesOptions);
            // should to call static files before mvc
            app.UseStaticFiles();
            app.UseAuthentication();

            //app.UseRouting();
            //app.UseEndpoints(builder => builder.MapControllers());
            // use mvc files
            //app.UseMvcWithDefaultRoute();
            //app.UseRouting();
            //app.UseCors();

            // replace UseDefaultFiles and UseStaticFiles to UseFileServer

            //app.UseFileServer(fileServerOptions);
            //app.UseFileServer();
            //app.UseMvcWithDefaultRoute();
            // app.UseRouting();
            //app.UseMvc();

            app.UseMvc(routes =>
            {
                // to makr the id optional add ?
                // to add the default page
                routes.MapRoute("dafault", "{controller=Home}/{action=Index}/{id?}");
            });


            //app.UseEndpoints(endpoints =>
            //{
            //    // endpoints.MapGet("/", async context =>
            //    // {
            //    //await context.Response.WriteAsync(_config["MyKey"]);
            //    // in debug output
            //    //logger.LogInformation("MW1 : Incoming Request");
            //    //throw new Exception("some Error processing ");

            //    // await context.Response.WriteAsync("Hi");
            //    //  });

            //    endpoints.MapControllerRoute(
            //    name: "default",
            //      pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

        }
    }
}
