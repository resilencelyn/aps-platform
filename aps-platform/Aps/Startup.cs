using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Services;
using Aps.Shared.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json.Converters;

namespace Aps
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
            services.AddMvcCore().AddApiExplorer()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddDbContextPool<ApsContext>(options =>
            {
                options.UseMySql("server = 121.5.26.37; database = ApsServer; user = root; password = zq19990821",
                    new MySqlServerVersion(new Version(5, 7, 30)), builder => { builder.CharSet(CharSet.Utf8Mb4); });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Aps",
                    Version = "v1",
                    Description = "排产平台API",
                    Contact = new OpenApiContact
                    {
                        Name = "张全",
                        Email = "zhangbig@outlook.com"
                    },
                    // Extensions = new Dictionary<string, IOpenApiExtension>
                    // {
                    //     {"xx", new }
                    // }
                });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "Aps.xml");
                c.IncludeXmlComments(filePath);
            });

            // services.Configure<ForwardedHeadersOptions>(options =>
            // {
            //     options.KnownProxies.Add(IPAddress.Parse("101.126.248.118"));
            // });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IAssemblyProcessRepository, AssemblyProcessRepository>();
            services.AddScoped<IScheduleTool, ScheduleTool>();

            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            services.AddTransient<IRepository<ApsManufactureProcess, string>, ManufactureProcessRepository>();

            services.AddTransient<IRepository<ApsOrder, string>, OrderRepository>();

            services.AddTransient<IRepository<ApsProduct, string>, ProductRepository>();
            services.AddTransient<IRepository<ApsSemiProduct, string>, SemiProductRepository>();

            services.AddTransient<IRepository<ApsResource, string>, ResourceRepository>();

            services.AddTransient<IRepository<ApsProcessResource, string>, ProcessResourceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    // c.PreSerializeFilters.Add((swagger, httpReq) =>
                    // {
                    //     //根据访问地址，设置swagger服务路径
                    //     swagger.Servers = new List<OpenApiServer>
                    //     {
                    //         new OpenApiServer
                    //         {
                    //             Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}"
                    //         }
                    //     };
                    // });
                });

                app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Aps.Net v1"); });
            }

            // app.UseHttpsRedirection();

            app.UseCors("Open");

            app.UseStaticFiles();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}