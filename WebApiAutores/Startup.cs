

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;

namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration) //Se inserta como una propiedad y eso genera una clas ede IConfigyration de get
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)//Proovedor de configuracion para acceder a la data de los proovedor
        {
            //Aqui se van a configurar los servicios
            //Se cortan los services de la parte de program
            services.AddControllers().AddJsonOptions (x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);//configuracion del json para desabilitar los cycles que inpiden la muestra de data

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //env es el ambiente
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        

        }

    }
}
