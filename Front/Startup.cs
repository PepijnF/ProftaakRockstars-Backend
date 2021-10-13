using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Proftaak.Data;
using Proftaak.Services;
using Proftaak.WebSocketModels;

namespace Proftaak
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proftaak", Version = "v1" });
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSingleton<LobbyContainer>();
            services.AddSingleton<UserContainer>();

            services.AddSingleton<ClientController>();
            services.AddSingleton<WebSockets>();


            //WebSockets server
            //WebSockets webSockets = new WebSockets(usercontainer => new UserContainer(provider.GetService<UserContainer>()), provider => new ClientController(provider.GetService<ClientController>()));
            //WebSockets webSockets = new WebSockets();


            //Verban de schrijver van deze code naar de diepste delen van de hel
            //WebSockets webSockets = new WebSockets((ClientController)services.ToList().Find(s => s.GetType() == typeof(ClientController)).ImplementationInstance, (UserContainer)services.ToList().Find(s => s.GetType() == typeof(UserContainer)).ImplementationInstance);
            //webSockets.StartServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proftaak v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(async (context) =>
            {
                
            });
            
            
        }
    }
}
