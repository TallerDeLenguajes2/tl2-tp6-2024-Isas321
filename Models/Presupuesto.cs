// using Microsoft.VisualBasic;

// namespace tl2_tp6_2024_Isas321.Models{
//   public class Presupuesto{
//     private int idPresupuesto;
//     private string nombreDestinatario;
//     private DateTime fechaCreacion;
//     public List<PresupuestoDetalle> detalles;

//     public Presupuesto()
//     {
//         this.Detalles = new List<PresupuestoDetalle>();
//     }

//         public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalles)
//         {
//             this.IdPresupuesto = idPresupuesto;
//             this.NombreDestinatario = nombreDestinatario;
//             this.FechaCreacion = fechaCreacion;
//             this.detalles = detalles;
//         }

//         public Presupuesto(string nombreDestinatario, DateTime fechaCreacion)
//         {
//             this.IdPresupuesto = 99;
//             this.NombreDestinatario = nombreDestinatario;
//             this.FechaCreacion = fechaCreacion;
//             this.detalles = new List<PresupuestoDetalle>();
//         }

//         public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
//         public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
//         public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
//         public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }
//     }
// }



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
