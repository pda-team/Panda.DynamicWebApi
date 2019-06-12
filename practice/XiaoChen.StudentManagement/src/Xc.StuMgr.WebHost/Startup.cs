using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Panda.DynamicWebApi;
using Swashbuckle.AspNetCore.Swagger;

namespace Xc.StuMgr.WebApiHost
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // 添加动态WebApi 需放在 AddMvc 之后
            services.AddDynamicWebApi();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "晓晨学生管理系统 WebApi", Version = "v1" });

                options.DocInclusionPredicate((docName, description) => true);

                options.IncludeXmlComments(@"bin\Debug\netcoreapp2.2\Xc.StuMgr.WebApiHost.xml");
                options.IncludeXmlComments(@"bin\Debug\netcoreapp2.2\Xc.StuMgr.Application.xml");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "晓晨学生管理系统 WebApi");
            });

            app.UseMvc();
        }
    }
}
