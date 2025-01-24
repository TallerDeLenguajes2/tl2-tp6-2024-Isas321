using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_Isas321.Models
{
    public class Cliente
    {   
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        public string Telefono { get; set; }


        public Cliente(int clienteId, string nombre, string email, string telefono)
        {
            ClienteId = clienteId;
            Nombre = nombre;
            Email = email;
            Telefono = telefono;
        }
    }


}
