using System;
using System.IO;
using System.Reflection;
using Beis.Htg.VendorSme.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using smevoucherencryption;
using VoucherCheckService.services.interfaces;
using VoucherCheckService.services.repositories;
using VoucherRedeemMicroService.common;
using VoucherRedeemMicroService.services;
using VoucherRedeemService.interfaces;

namespace VoucherUpdateService
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
            services.Configure<EncryptionSettings>(options =>
                Configuration.Bind(options));

            services.AddLogging(options =>
            {
                // hook the Console Log Provider
                options.AddConsole();
                options.SetMinimumLevel(LogLevel.Trace);

            });
            services.AddApplicationInsightsTelemetry(Configuration["AZURE_MONITOR_INSTRUMENTATION_KEY"]);

            services.AddControllers();
            services.AddSingleton<IEncryptionService, AesEncryption>();            

            services.AddDbContext<HtgVendorSmeDbContext>(options => options.UseNpgsql(Configuration["HELPTOGROW_CONNECTIONSTRING"]), ServiceLifetime.Transient);

            services.AddTransient<IVoucherRedeemService, VoucherRedeemMicroService.services.VoucherRedeemService>();
            services.AddTransient<IVendorAPICallStatusServices, VendorAPICallStatusServices>();
            services.AddTransient<IVendorAPICallStatusRepository, VendorApiCallStatusRepository>();

            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IVendorCompanyRepository, VendorCompanyRepository>();
            services.AddTransient<IEnterpriseRepository, EnterpriseRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "beis-htg-sme-voucher-redeem-service", Version = "v1"});
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "beis-htg-sme-voucher-redeem-service v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}