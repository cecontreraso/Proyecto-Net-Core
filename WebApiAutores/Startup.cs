

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using WebApiAutores.Controllers;
using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;
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
            services.AddControllers(opcions =>
            {
                opcions.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions (x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);//configuracion del json para desabilitar los cycles que inpiden la muestra de data
            //Servicio no es mas que una dependencia
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddTransient<IServicio, ServicioA>();//que cuando una clase requiera un iservicio pasa a un servicioA

            //services.AddScoped<>; tiempo de vida de la clase de ServicioA, se nos va a dar la misma instancia, pero en distintas peticiones dan distintas instancias
            //services.AddSingleton<> siempre tenemos la misma instancia, independiente del tiempo y las peticiones


            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();

            //servicio de mi filtro
            services.AddTransient<MiFiltroDeAccion>();

            //servicio del catching
            services.AddResponseCaching();

            //servicio de autentificacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) //env es el ambiente
        {

            //app.UseMiddleware<LoguearRespuestaHTTPMiddlewares>();
            app.UseLoguearRespuestaHTTP(); //exactamente lo mismo, pero escondiendo de que clase viene el metodo

            app.Map("/ruta1", app => //Map hace una difurcancion de la tuberia de procesos. 
            {
                app.Run(async contexto =>
                {
                    await contexto.Response.WriteAsync("Estoy interseptando la tuberia");
                });
            });

            

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();//milddlware para usar cache de datos


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        

        }

    }
}
