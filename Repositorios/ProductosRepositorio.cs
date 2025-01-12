using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        public List<Producto> GetAll()
        {
            List<Producto> productos = new List<Producto>();
            var cadena = "Data Source = db/Tienda.db";

            using (var sqlitecon = new SqliteConnection(cadena))
            {
                sqlitecon.Open();
                var consulta = "SELECT * FROM Productos;";
                var command = new SqliteCommand(consulta, sqlitecon);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var IdProducto = Convert.ToInt32(reader["idProducto"]);
                    var Descripcion = reader["Descripcion"].ToString();
                    var Precio = Convert.ToDouble(reader["Precio"]); // Asegúrate del tipo correcto
                    var producto = new Producto(IdProducto, Descripcion, Precio);
                    productos.Add(producto);
                }

                sqlitecon.Close(); // Liberar conexión
            }

            return productos;
        }
    }
}
