using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]

    public class LibrosController:ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]//
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await _context.Libros.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor =await _context.Autores.AnyAsync(x => x.Id != libro.AutorID);//Verificar que el autor exista

            if(!existeAutor) 
            {
                return BadRequest($"No existe el autor de Id: {libro.AutorID}");
            }
            else
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return Ok();
            }

        }

    }



}
