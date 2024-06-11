using System.ComponentModel.DataAnnotations;

namespace AppLogin.ViewModels
{
    //Clase de pasos
    public class UserVM
    {
        [RegularExpression(@"^[A-Z'\s]{1,40}$",
            ErrorMessage = "Necesita estar en mayusculas")]
        [StringLength(40)]
        [Required(ErrorMessage = "Este dato es importante por favor llene")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Este campo es importante para poder acceder al sistema, por favor completelo")]
        [EmailAddress(ErrorMessage = "Este dato es importante por favor llene correctamente")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Este dato es importante por su seguridad por favor llene")]
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}