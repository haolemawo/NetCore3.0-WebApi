using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace XXX.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ������������ע�����,ע��õķ�������������ط����е���
        public void ConfigureServices(IServiceCollection services)
        {
            //ע��swagger����,����1�����߶��swagger�ĵ�
            services.AddSwaggerGen(s=> {
                //����swagger�ĵ������Ϣ
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "xxxWebApi�ĵ�",
                    Description = "����һ���򵥵�NetCore WebApi��Ŀ",
                    Version = "v1.0"
                });
            
                //��ȡxmlע���ļ���Ŀ¼
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ����xmlע��
                s.IncludeXmlComments(xmlPath);
            });
            services.AddControllers(option=> {
                //�����쳣������
                option.Filters.Add(new MyExceptionFilter());
            });
            services.AddRouting();
        }

        // ���������м���ܵ�,�������Ӧhttp����.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            
            //��appsettings.json��ȡ�����ļ�
            Common.AppSettings.SetAppSetting(Configuration.GetSection("AppSettings"));
            app.UseRouting();

            app.UseAuthorization();
            //����swagger�м��
            app.UseSwagger(opt=> {
                //opt.RouteTemplate = "api/{controller=Home}/{action=Index}/{id?}";
            });
            //����SwaggerUI�м����htlm css js�ȣ�������swagger json ���
            app.UseSwaggerUI(s => {
                
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "xxxWebapi�ĵ�v1");

                //Ҫ��Ӧ�õĸ� (http://localhost:<port>/) ���ṩ Swagger UI���뽫 RoutePrefix ��������Ϊ���ַ�����
                //s.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //     name: "default",
                //     pattern: "api/{controller}/{action}/{id?}");
                endpoints.MapControllers();
                
            });

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            //});
        }
    }
}
