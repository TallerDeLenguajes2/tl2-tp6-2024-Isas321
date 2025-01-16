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


        public void Create(Producto producto)
        {
            var cadena = "Data Source = db/Tienda.db";
            using( var sqlitecon = new SqliteConnection(cadena)){
            // Importante crear y destruir!
            sqlitecon.Open();
            var consulta = @"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";
            SqliteCommand comand = new SqliteCommand(consulta, sqlitecon);
            // Agregar parámetros para evitar inyecciones SQL
            comand.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            comand.Parameters.AddWithValue("@Precio", producto.Precio);
            // Ejecutar el comando
            comand.ExecuteNonQuery();
            sqlitecon.Close(); //Me aseguro que la BD queda liberada
            }
        }

   
        // public Producto GetProductoPorId(int id)
        // {
        //     if (id <= 0)
        //     {
        //         throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
        //     }

        //     var connectionString = "Data Source=db/Tienda.db";
        //     using (var sqliteConnection = new SqliteConnection(connectionString))
        //     {
        //         sqliteConnection.Open();
        //         const string query = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id";
                
        //         using (var command = new SqliteCommand(query, sqliteConnection))
        //         {
        //             // Usar un parámetro para evitar inyecciones SQL
        //             command.Parameters.AddWithValue("@id", id);

        //             using (var reader = command.ExecuteReader())
        //             {
        //                 if (reader.Read())
        //                 {
        //                     var productoId = reader.GetInt32(reader.GetOrdinal("idProducto"));
        //                     var descripcion = reader["Descripcion"] as string ?? "Sin descripción";
        //                     var precio = reader.GetInt32(reader.GetOrdinal("Precio"));
                            
        //                     return new Producto(productoId, descripcion, precio);
        //                 }
        //                 else
        //                 {
        //                     throw new InvalidOperationException($"No se encontró el producto con ID {id}");
        //                 }
        //             }
        //         }     
        //     }
        // }

        public bool Remove(int id)
        {
            int rowsAffected;
            var connectionString = "Data Source=db/Tienda.db";
            using (var sqliteConnection = new SqliteConnection(connectionString))
            {
                const string query = "DELETE FROM Productos WHERE idProducto = @id";
                using (var command = new SqliteCommand(query, sqliteConnection))
                {
                    sqliteConnection.Open();
                    command.Parameters.AddWithValue("@id", id);
                    rowsAffected = command.ExecuteNonQuery();
                    sqliteConnection.Close();
                    return rowsAffected==1;
                }
            }
        }

        // public void ActualizarNombrePorId(int id, string descripcion)
        // {
        //     if (id <= 0)
        //     {
        //         throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
        //     }

        //     var connectionString = "Data Source=db/Tienda.db";
        //     using (var sqliteConnection = new SqliteConnection(connectionString))
        //     {
        //         sqliteConnection.Open();
        //         const string query = "UPDATE Productos SET Descripcion = @descripcion WHERE idProducto = @id";
                
        //         using (var command = new SqliteCommand(query, sqliteConnection))
        //         {
        //             // Agregar el parámetro de ID para evitar inyección SQL
        //             command.Parameters.AddWithValue("@descripcion", descripcion);
        //             command.Parameters.AddWithValue("@id", id);

        //             // Ejecutar el comando de eliminación
        //             int rowsAffected = command.ExecuteNonQuery();

        //             if (rowsAffected == 0)
        //             {
        //                 // Manejo si no se encontró el producto para eliminar
        //                 throw new KeyNotFoundException($"No se encontró ningún producto con ID {id} para eliminar.");
        //             }
        //         }
        //     }
        // }


        // //Id en ruta y producto en el cuerpo
        // public bool ActualizarProducto(int id, Producto producto)
        // {
        //     if (id <= 0)
        //     {
        //         return false;
        //     }

        //     var descripcion = producto.Descripcion;
        //     var precio = producto.Precio;

        //     var connectionString = "Data Source=db/Tienda.db";
        //     using (var sqliteConnection = new SqliteConnection(connectionString))
        //     {
        //         sqliteConnection.Open();
        //         const string query = "UPDATE Productos SET Descripcion = @descripcion, Precio = @precio WHERE idProducto = @id";
                
        //         using (var command = new SqliteCommand(query, sqliteConnection))
        //         {
        //             // Agregar el parámetro de ID para evitar inyección SQL
        //             command.Parameters.AddWithValue("@descripcion", descripcion);
        //             command.Parameters.AddWithValue("@precio", precio);
        //             command.Parameters.AddWithValue("@id", id);

        //             int rowsAffected = command.ExecuteNonQuery();

        //             if (rowsAffected == 0)
        //             {
        //                 return false;
        //             }
        //             return true;
        //         }
        //     }
        // }

    }
}
