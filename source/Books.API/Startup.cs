using BooksStore.Bll;
using BooksStore.CacheDal;
using BooksStore.CacheDal.Persistence;
using BooksStore.Core.Configuration;
using BooksStore.Core.Interfaces;
using BooksStore.SqlDal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Authority"];
                    options.Audience = Configuration["Auth0:Audience"];
                });

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

            services.Configure<DataStoreSettings>(Configuration.GetSection(nameof(DataStoreSettings)));
            services.AddSingleton<IDataStoreSettings>(sp => sp.GetRequiredService<IOptions<DataStoreSettings>>().Value);

            services.AddSingleton<IRedisCacheDbContext, RedisCacheDbContext>();
            services.AddScoped<IBooksBll, BooksBll>();
            services.AddScoped<IBookCacheRepository, BookCacheRepository>();

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

}

// Configuration["ConnectionStrings:SqlServerConnection"]
// SQL database connection (name defined in appsettings.json).
//var settingsData = new SettingsData(Configuration.GetConnectionString("SqlServerConnection"));
//services.AddSingleton<IDataStoreSettings>(settingsData);


//// Redis Cache Dependencies
//services.AddSingleton<ConnectionMultiplexer>(sp =>
//{
//    var configuration = ConfigurationOptions.Parse(Configuration["ConnectionStrings:RedisConnectionString"], true);
//    return ConnectionMultiplexer.Connect(configuration);
//});
