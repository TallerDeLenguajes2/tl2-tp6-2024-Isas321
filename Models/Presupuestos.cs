using Microsoft.VisualBasic;

namespace MiWebAPI.Models{
  public class Presupuesto{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;

    public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    
    public string MostrarPresupuesto(){
      return $"{idPresupuesto} {nombreDestinatario} {fechaCreacion}";
    }
  }
}