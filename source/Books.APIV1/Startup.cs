using Books.APIV1.Configuration;
using Books.APIV1.Interfaces;
using Books.APIV1.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Books.APIV1
{

    public class Startup
    {

        public IConfiguration Configuration { get; }
        const string _corsPolicyName = "AllHosts";
        const string _xmlCommentsFileName = "Books.APIV1.xml";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicyName, builder => builder.AllowAnyOrigin()
                                                            .AllowAnyHeader()
                                                             .AllowAnyMethod());
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Books.APIV1", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, _xmlCommentsFileName);

                c.IncludeXmlComments(filePath);
            });

            // Configuration["ConnectionStrings:SqlServerConnection"]
            // SQL database connection (name defined in appsettings.json).
            var dataStoreSettings = new DataStoreSettings(Configuration.GetConnectionString("SqlServerConnection"));
            services.AddSingleton(dataStoreSettings);

            services.AddScoped<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books.APIV1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_corsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

}
