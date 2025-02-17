namespace tl2_tp6_2024_Isas321.Models
{
    public class CrearPresupuestoViewModel
    {
        public DateTime FechaCreacion { get; set; } // Fecha de creación del presupuesto
        public List<Cliente> Clientes { get; set; } // Lista de clientes para seleccionar
        public int ClienteId { get; set; } // ID del cliente seleccionado para el presupuesto

        // Constructor vacío
        public CrearPresupuestoViewModel()
        {
            Clientes = new List<Cliente>();
        }

        // Constructor con parámetros para inicializar los datos
        public CrearPresupuestoViewModel(DateTime fechaCreacion, List<Cliente> clientes)
        {
            FechaCreacion = fechaCreacion;
            Clientes = clientes ?? throw new ArgumentNullException(nameof(clientes));
        }
    }



}
