using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PaymentGateway.Common.Interfaces;
using PaymentGateway.Infrastructure.Repositories;
using PaymentGateway.Services.Services;
using System;
using System.IO;
using System.Reflection;
using PaymentGateway.Common.Constants;
using Prometheus;
using PaymentGateway.API.ServiceExtensions;

namespace PaymentGateway.API
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
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentGatewayAPI", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IPaymentValidators, PaymentValidators>();

            var bankHttpRepository = Configuration["BankHttpRepository"] ?? "";            
            if (bankHttpRepository == ConstantValues.MockedHttpBankRepository)
                services.AddTransient<IBankHttpRepository, MockedBankHttpRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpMetrics(PrometheusConfig.Options);
            app.UseMetricServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
