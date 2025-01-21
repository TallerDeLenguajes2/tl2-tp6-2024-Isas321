namespace tl2_tp6_2024_Isas321.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestoDetalle> Detalles { get; set; } = new List<PresupuestoDetalle>();

        // Constructor sin parámetros (necesario para frameworks como Entity Framework)
        public Presupuesto() { }

        // Constructor para crear un presupuesto básico
        public Presupuesto(string nombreDestinatario, DateTime fechaCreacion)
        {
            NombreDestinatario = nombreDestinatario;
            FechaCreacion = fechaCreacion;
        }

        // Constructor completo
        public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalles)
        {
            IdPresupuesto = idPresupuesto;
            NombreDestinatario = nombreDestinatario ?? throw new ArgumentNullException(nameof(nombreDestinatario));
            FechaCreacion = fechaCreacion;
            Detalles = detalles ?? throw new ArgumentNullException(nameof(detalles));
        }
    }
}
