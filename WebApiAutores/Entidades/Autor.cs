using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido"/*esto es un placeholder)*/)]//Esto hace que el campo nombre sea siempre requerido
        [StringLength(maximumLength:5, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        //[Range(18,100)]
        //[NotMapped]//esto sirve para propiedades, pero no con la de las columnas de las tablas correspondientes
        //public int Edad {get; set; }
        //[CreditCard]
        //[NotMapped]
        //public string TarjetaCredito { get; set; }
        //[Url]
        //[NotMapped]
        //public string URL { get; set; }

        /*public int Menor { get; set; }
        public int Mayor { get; set; }*/

        public List<Libro> Libros { get; set; }
        


        //Validaciones por modelo de datos
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Nombre))
                {
                    var primeraLetra = Nombre[0].ToString();
                    if(primeraLetra != primeraLetra.ToUpper())
                    {
                        yield return new ValidationResult("La primera letra debe ser mayuscula",//yiel se inserta un elemento en la coleccion 
                            new string[] { nameof(Nombre) });
                    }
                }

  /*          if(Menor > Mayor)
            {
                yield return new ValidationResult("Este valor no puede ser mas grande que el campo Mayor",
                    new string[] {nameof(Menor)});
            }*/

        }

        //Reglas de validacion por modelo
    }
}
