using libraryManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement
{
    public class Startup
    {
        //To add the database connection string we need to implement Iconfiguration Interface. It will be static as we will not instantiate it.
        //This allow us to add connection string in appjson file.
        public static IConfiguration Configuration { get; set; }
        //now we like to inject this interface into our startup class (dependency injection). To do that we pass it as argument in constructor

        public Startup (IConfiguration configuration) //passing the interface direct into constructor and value of it can be assign to a property
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        //we setup the connetionStrings from the configuration property that we set and Iconfiguration interface. Now we define the strings "connectionStrings"
        //it can be anything
            var connectionString = Configuration["connectionStrings:libraryDbConnectionString"];

            // To create the Database we need to inject the AddDbContext services. then run the migration from package Manager Console
            //from Tools-NuGet Pakage Manager-Package Manger Console.
            services.AddDbContext<LibraryDbContext>(connection => connection.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //to add the seed data to database we need to add Dbcontext in this method. so first add the argument after env then call the method created in DbSeedingClass
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LibraryDbContext context) 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            context.SeedDataContext();   //call seedDataContext method

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //we will use mvc controller so replace endpoint.mapget with default route
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}"
              );
            });
        }
    }
}
