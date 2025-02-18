using System;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_Isas321.Models
{
    public class Producto
    {
        private int idProducto;
        private string descripcion;
        private decimal precio;
        public Producto() { }
        public Producto(string descripcion, decimal precio)
        {
            this.descripcion = descripcion;
            this.precio = precio;
        }
        public Producto(int idProducto, string descripcion, decimal precio)
        {
            this.idProducto = idProducto;
            this.descripcion = descripcion;
            this.precio = precio;
        }

        public int IdProducto { get => idProducto; set => idProducto = value; }

        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string Descripcion { get => descripcion; set => descripcion = value; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get => precio; set => precio = value; }

        public string MostrarProducto()
        {
            return $"\n\tIdProducto [id]: {idProducto}\n\tDescripción: {descripcion}\n\tPrecio: {precio}\n";
        }
    }
}
