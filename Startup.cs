using Microsoft.AspNetCore.Builder;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WSVenta
{
    public class Startup
    {
        readonly string MiCors = "MiCors"; //agregado para la configuración con Angular
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //agregado para la configuración con Angular
            services.AddCors(option =>
            {
                option.AddPolicy(name: MiCors, builder =>
                {
                    //builder.WithOrigins("*");
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyHeader().AllowAnyMethod();
                });
            }); 

            services.AddControllers(); //este ya estaba
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MiCors);//agregado para la configuración con Angular
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers(); 
            });
        }
    }
}
