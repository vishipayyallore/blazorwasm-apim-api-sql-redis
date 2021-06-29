using BooksStore.CacheDal;
using BooksStore.CacheDal.Interfaces;
using BooksStore.CacheDal.Persistence;
using BooksStore.Core.Configuration;
using BooksStore.SqlDal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.IO;

namespace Books.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        const string _corsPolicyName = "AllHosts";
        const string _xmlCommentsFileName = "Books.API.xml";

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Books.API", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, _xmlCommentsFileName);

                c.IncludeXmlComments(filePath);
            });

            // Configuration["ConnectionStrings:SqlServerConnection"]
            // SQL database connection (name defined in appsettings.json).
            //var settingsData = new SettingsData(Configuration.GetConnectionString("SqlServerConnection"));
            //services.AddSingleton<IDataStoreSettings>(settingsData);
            services.Configure<DataStoreSettings>(Configuration.GetSection(nameof(DataStoreSettings)));
            services.AddSingleton<IDataStoreSettings>(sp => sp.GetRequiredService<IOptions<DataStoreSettings>>().Value);

            //// Redis Cache Dependencies
            //services.AddSingleton<ConnectionMultiplexer>(sp =>
            //{
            //    var configuration = ConfigurationOptions.Parse(Configuration["ConnectionStrings:RedisConnectionString"], true);
            //    return ConnectionMultiplexer.Connect(configuration);
            //});

            services.AddSingleton<IRedisCacheDbContext, RedisCacheDbContext>();
            services.AddScoped<IBookCacheRepository, BookCacheRepository>();

            // SQL Server Dependencies
            services.AddScoped<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books.API v1"));
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
