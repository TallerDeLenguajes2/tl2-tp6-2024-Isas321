namespace tl2_tp6_2024_Isas321.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestoDetalle> Detalles { get; set; } = new List<PresupuestoDetalle>();
        public Cliente Cliente { get; set; } // Referencia al cliente asociado

        // Constructor sin parámetros (necesario para frameworks como Entity Framework)
        public Presupuesto() { }

        // Constructor básico sin detalles
        public Presupuesto(Cliente cliente, DateTime fechaCreacion)
        {
            Cliente = cliente;
            FechaCreacion = fechaCreacion;
        }

        // Constructor completo
        public Presupuesto(int idPresupuesto, Cliente cliente, DateTime fechaCreacion, List<PresupuestoDetalle> detalles)
        {
            IdPresupuesto = idPresupuesto;
            Cliente = cliente;
            FechaCreacion = fechaCreacion;
            Detalles = detalles ?? throw new ArgumentNullException(nameof(detalles));
        }
    }
}
