using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]//Atibuto, hace validaciones automaticas respecto a la data recibida
    [Route("api/autores")]//Ruta de la API
    public class AutoresController: ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Autor>> Get()
        {
            return new List<Autor>()
            {

                new Autor() { Id = 1, Nombre = "Cesar"},
                new Autor() { Id = 2, Nombre = "Felipe"}
            };
                

            
        }

    }
}
