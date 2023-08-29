

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using WebApiAutores.Controllers;
using WebApiAutores.Servicios;

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
            //Servicio no es mas que una dependencia
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddTransient<IServicio, ServicioA>();//que cuando una clase requiera un iservicio pasa a un servicioA

            //services.AddScoped<>; tiempo de vida de la clase de ServicioA, se nos va a dar la misma instancia, pero en distintas peticiones dan distintas instancias
            //services.AddSingleton<> siempre tenemos la misma instancia, independiente del tiempo y las peticiones


            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();

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
