using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Validaciones
{
    public class PrimeraLetraMayusculaAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
                {
                    return base.IsValid(value, validationContext);
                }

            var primeraLetra = value.ToString()[0].ToString();

            if(primeraLetra != primeraLetra.ToUpper()) 
            {
                return new ValidationResult("La primer aletra debe ser mayuscula");

            }

            return ValidationResult.Success;
        }
    }
}
