namespace MiWebAPI.Models{
  public class PresupuestoDetalle{
    private int idPresupuesto;
    private int idProducto;
    private int cantidad;

        public PresupuestoDetalle(int idPresupuesto, int idProducto, int cantidad)
        {
            this.idPresupuesto = idPresupuesto;
            this.idProducto = idProducto;
            this.cantidad = cantidad;
        }

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public int IdProducto { get => idProducto; set => idProducto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
    
        public string MostrarPresupuesto(){
      return $"{idPresupuesto} {idProducto} {cantidad}";
    }
    }
}