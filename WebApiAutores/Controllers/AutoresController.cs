using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]//Atibuto, hace validaciones automaticas respecto a la data recibida
    [Route("api/autores")]//Ruta de la API
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AutoresController(ApplicationDbContext context)//constructor de la clase
        {
            this._context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            //return new List<Autor>()
            //{

            //    new Autor() { Id = 1, Nombre = "Cesar"},
            //    new Autor() { Id = 2, Nombre = "Felipe"}
            //};

            return await _context.Autores.ToListAsync();


        }
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
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

            var existeAutor = await _context.Autores.AnyAsync(x => x.Id == id);
            if (!existeAutor)
            {
                return NotFound();
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
