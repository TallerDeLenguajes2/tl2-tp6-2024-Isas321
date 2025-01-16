namespace tl2_tp6_2024_Isas321.Models{
  public class PresupuestoDetalle{
    private Producto producto;
    private int cantidad;

        public PresupuestoDetalle(Producto producto, int cantidad)
        {
            this.Producto = producto;
            this.Cantidad = cantidad;
        }

        public Producto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
    }
}