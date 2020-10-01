using Azure.Storage.Blobs;
using IoTExcercise.DataAccess;
using IoTExcercise.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IoTExcercise
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
            services.AddSingleton(s => 
                new AzureBlobContext(new BlobServiceClient(Configuration.GetConnectionString("AzureBlob")), 
                Configuration.GetValue<string>("AzureBlobContainerName"))
                );
            services.AddScoped<ISensorsDataRepo, SensorsDataRepo>();
            services.AddScoped<ISensorsService, SensorsService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
