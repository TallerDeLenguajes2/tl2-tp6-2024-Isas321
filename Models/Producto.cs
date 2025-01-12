namespace tl2_tp6_2024_Isas321.Models
{
    // public class ProductoCreateModel
    // {
    //     public string Descripcion { get; set; }
    //     public int Precio { get; set; }
    // }
    public class Producto
    {
        private int idProducto;
        private string descripcion;
        private double precio;

        // Constructor vacío
        public Producto() { }

        // Constructor sin ID la base de datos lo autoincrementará
        public Producto(string descripcion, double precio)
        {
            this.descripcion = descripcion;
            this.precio = precio;
        }

        public Producto(int idProducto, string descripcion, double precio)
        {
            this.idProducto = idProducto;
            this.descripcion = descripcion;
            this.precio = precio;
        }

        // Propiedades
        public int IdProducto { get => idProducto; set => idProducto = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public double Precio { get => precio; set => precio = value; }

        // Método para mostrar detalles del producto
        public string MostrarProducto()
        {
            return $"\n\tIdProducto [id]: {idProducto}\n\tDescripcion: {descripcion}\n\tPrecio: {precio}\n";
        }
    }
}
