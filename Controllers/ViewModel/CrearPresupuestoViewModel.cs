namespace tl2_tp6_2024_Isas321.Models
{
    public class CrearPresupuestoViewModel
    {
        public DateTime FechaCreacion { get; set; } 
        public List<Cliente> Clientes { get; set; } 
        public int ClienteId { get; set; }

        public CrearPresupuestoViewModel()
        {
            Clientes = new List<Cliente>();
        }

        public CrearPresupuestoViewModel(DateTime fechaCreacion, List<Cliente> clientes)
        {
            FechaCreacion = fechaCreacion;
            Clientes = clientes ?? throw new ArgumentNullException(nameof(clientes));
        }
    }
}
