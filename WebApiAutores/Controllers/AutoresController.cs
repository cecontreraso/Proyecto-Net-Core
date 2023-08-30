using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]//Atibuto, hace validaciones automaticas respecto a la data recibida
    [Route("api/autores")]//Ruta de la API [controller] usa el prefijo de la clase del controller

    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ServicioScoped servicioScoped;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio, ServicioTransient servicioTransient, 
            ServicioSingleton servicioSingleton, ServicioScoped servicioScoped, ILogger<AutoresController> logger)//constructor de la clase
        {
            this._context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioSingleton = servicioSingleton;
            this.servicioScoped = servicioScoped;
            this.logger = logger;
        }


        [HttpGet]//api/autores
        [HttpGet("listado")]//api/autores/listado
        [HttpGet("/listado")]//ruta especifica reemplazada al api/autores, por /listado
        [ResponseCache(Duration = 10)]
        [Authorize]
        public async Task<ActionResult<List<Autor>>> Get()
        {

            logger.LogInformation("Estamos obteniendo los autores");
            return await _context.Autores.Include(x => x.Libros).ToListAsync();//para incluir la informacion de los libros en el get de los autores


        }


        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]//se va a guardar el cache de la ultima peticion durante 10 segundos
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {
                AutoresControllerTransiet = servicioTransient.Guid,
                AutoresControllerScoped = servicioScoped.Guid,
                AutoresControllerSingleton = servicioSingleton.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                ServicioA_Scoped = servicio.ObtenerScoped(),
                ServicioA_Singleton = servicio.ObtenerSinglenton()


            });
        }


        [HttpGet("primero")]//De esta manera se le da una ruta especifica al metodo api/autores/primero?nombre=felipe&apellido=gavilar
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)//especifica que el valor lo va a buscar el header
        {


            return await _context.Autores.FirstOrDefaultAsync();//para incluir la informacion de los libros en el get de los autores
        }

        [HttpGet("{id:int}/(param2=persona)")]//esto se hace para que pueda buscar por dos parametros y el signo de interrgocacion hace que el segundo parametro es opcional
        public async Task<ActionResult<Autor>> Get(int id, string param2)
        {
             var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null) 
            {
                return NotFound();
            }
            else
            {
                return autor;
            }
        }

        [HttpPost]//Con [FromBody especificamos que la informacion viene el body]
        public async Task<ActionResult> Post([FromBody]Autor autor)
        {

            var existeAutorConMismoNombre = await  _context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);//Ve a la tabla de autores y dime si hay una coincidencia

            if (existeAutorConMismoNombre)
            {
                return BadRequest($"Existe ya un autor con el mismo nombre {autor.Nombre}");
            }


            _context.Add(autor);
            await _context.SaveChangesAsync();//guardar los cambios de maner asyncrona
            return Ok();
        }

        [HttpPut("{id:int}")]//para llamar a este httpPut es codigo seria api/autores/algo
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");//BadRequest, error:400
            }
            else
            {
                _context.Update(autor);
                await _context.SaveChangesAsync();
                return Ok();
            }

        }

        [HttpDelete("{id:int}")]//para llamar a este http seria api/autores/algo
        public async Task<ActionResult> Delete( int id) 
        { 
            var existeAutor = await _context.Autores.AnyAsync(x => x.Id == id);
            if (!existeAutor) 
            {
                return NotFound();
            }

            _context.Remove(new Autor()
            {
                Id = id
            });
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
