namespace tl2_tp6_2024_Isas321.Models
{
    public class CrearPresupuestoViewModel
    {
        public DateTime FechaCreacion { get; set; } // Fecha de creación del presupuesto
        public List<Cliente> Clientes { get; set; } // Lista de clientes para seleccionar
        public Cliente ClienteSeleccionado { get; set; } // Cliente seleccionado para el presupuesto
        public List<PresupuestoDetalle> Detalles { get; set; } = new List<PresupuestoDetalle>(); // Detalles de los productos agregados

        // Constructor vacío
        public CrearPresupuestoViewModel()
        {
            Clientes = new List<Cliente>();
            Detalles = new List<PresupuestoDetalle>();
        }

        // Constructor con parámetros para inicializar los datos
        public CrearPresupuestoViewModel(DateTime fechaCreacion, List<Cliente> clientes)
        {
            FechaCreacion = fechaCreacion;
            Clientes = clientes ?? throw new ArgumentNullException(nameof(clientes));
            Detalles = new List<PresupuestoDetalle>();
        }
    }


}
