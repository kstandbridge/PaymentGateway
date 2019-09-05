using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using PaymentGateway.BankService;
using PaymentGateway.Contracts;
using PaymentGateway.Data;
using PaymentGateway.Domain;
using PaymentGateway.Processing;

namespace PaymentGateway
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

            services.AddOpenApiDocument(config =>   // Add OpenAPI v3 document for NSwag
            {
                config.Title = "Payment Gateway";
            });

            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<ICommandQueue<SubmitPaymentCommand>, InMemoryCommandQueue<SubmitPaymentCommand>>();
            services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
            services.AddSingleton<IBankServiceClient, FakeBankServiceClient>();
            services.AddScoped<IPaymentManager, PaymentManager>();
            services.AddHostedService<CreatePaymentProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseOpenApi();       // Serve OpenAPI/NSwag documents
            app.UseSwaggerUi3();    // Server NSwag UI
            app.UseMvc();
        }
    }
}
