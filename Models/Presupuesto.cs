using Microsoft.VisualBasic;

namespace tl2_tp6_2024_Isas321.Models{
  public class Presupuesto{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    public List<PresupuestoDetalle> detalles;

        public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalles)
        {
            this.IdPresupuesto = idPresupuesto;
            this.NombreDestinatario = nombreDestinatario;
            this.FechaCreacion = fechaCreacion;
            this.detalles = detalles;
        }

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }
    }
}