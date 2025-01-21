using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios
{
    public class PresupuestoRepositorio : IPresupuestoRepositorio
            {
        public int CrearPresupuestoVacio(Presupuesto presupuesto)
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            if (string.IsNullOrWhiteSpace(presupuesto.NombreDestinatario))
            {
                throw new ArgumentException("El nombre del destinatario no puede estar vacío.", nameof(presupuesto.NombreDestinatario));
            }
            if (presupuesto.FechaCreacion == default)
            {
                throw new ArgumentException("La fecha de creación no es válida.", nameof(presupuesto.FechaCreacion));
            }

            int idPresupuesto = 0;

            try
            {
                using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
                {
                    sqlitecon.Open();

                    var consultaPresupuesto = @"
                        INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                        VALUES (@NombreDestinatario, @FechaCreacion);
                        SELECT last_insert_rowid();";

                    using (var commandPresupuesto = new SqliteCommand(consultaPresupuesto, sqlitecon))
                    {
                        commandPresupuesto.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
                    
                        commandPresupuesto.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion.ToString("yyyy-MM-dd"));

                        object resultado = commandPresupuesto.ExecuteScalar();
                        if (resultado != null && int.TryParse(resultado.ToString(), out int id))
                        {
                            idPresupuesto = id;
                        }
                        else
                        {
                            throw new InvalidOperationException("No se pudo obtener el ID del presupuesto insertado.");
                        }
                    }
                }
            }
            catch (SqliteException sqliteEx)
            {
                Console.WriteLine($"Error en la base de datos: {sqliteEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return idPresupuesto;
        }


        public List<Presupuesto> ObtenerTablaPresupuestos()
        {
            var _cadenaDeConexion = "Data Source = db/Tienda.db";
            var presupuestos = new List<Presupuesto>();
            var consulta = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion
                            FROM Presupuestos";

            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        string nombreDestinatario = Convert.ToString(reader["NombreDestinatario"]);
                        DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]).Date;
                        var presupuesto = new Presupuesto(idPresupuesto, nombreDestinatario, fechaCreacion, new List<PresupuestoDetalle>());
                        presupuestos.Add(presupuesto);
                    }
                }
                sqlitecon.Close();
            }
            return presupuestos;
        }


        public Presupuesto ObtenerTablaPresupuestosPorId(int id)
        {
            var _cadenaDeConexion = "Data Source = db/Tienda.db";
            Presupuesto presupuesto = null;
            var consulta = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion
                            FROM Presupuestos
                            WHERE idPresupuesto = @id";

            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        string nombreDestinatario = Convert.ToString(reader["NombreDestinatario"]);
                        DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]).Date;
                        presupuesto = new Presupuesto(idPresupuesto, nombreDestinatario, fechaCreacion, new List<PresupuestoDetalle>());
                    }
                }
                sqlitecon.Close();
            }
            return presupuesto;
        }
            
    


        public List<Presupuesto> ObtenerPresupuestoCompleto()
        {
            var _cadenaDeConexion = "Data Source = db/Tienda.db";
            var presupuestos = new List<Presupuesto>();
            var consulta = @"SELECT Pres.idPresupuesto, Pres.NombreDestinatario, Pres.FechaCreacion, 
                                    Prod.idProducto, Prod.Descripcion, Prod.Precio, PresD.Cantidad
                            FROM Presupuestos Pres
                            LEFT JOIN PresupuestosDetalle PresD ON Pres.idPresupuesto = PresD.idPresupuesto
                            LEFT JOIN Productos Prod ON Prod.idProducto = PresD.idProducto";


            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
                using (var reader = command.ExecuteReader())
                {
                    var presupuestoDict = new Dictionary<int, Presupuesto>();

                    while (reader.Read())
                    {
                        int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        string nombreDestinatario = Convert.ToString(reader["NombreDestinatario"]);
                        DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]).Date;

                        int? idProducto = reader["idProducto"] != DBNull.Value ? Convert.ToInt32(reader["idProducto"]) : (int?)null;
                        string descripcion = reader["Descripcion"] != DBNull.Value ? Convert.ToString(reader["Descripcion"]) : null;
                        double? precio = reader["Precio"] != DBNull.Value ? Convert.ToDouble(reader["Precio"]) : (double?)null;
                        int? cantidad = reader["Cantidad"] != DBNull.Value ? Convert.ToInt32(reader["Cantidad"]) : (int?)null;

                        Producto producto = null;
                        PresupuestoDetalle presupuestoDetalle = null;

                        if (idProducto.HasValue)
                        {
                            producto = new Producto(idProducto.Value, descripcion, precio ?? 0);
                            presupuestoDetalle = new PresupuestoDetalle(producto, cantidad ?? 0);
                        }

                        // Verificar si el presupuesto ya está en el diccionario
                        if (!presupuestoDict.ContainsKey(idPresupuesto))
                        {
                            presupuestoDict[idPresupuesto] = new Presupuesto(idPresupuesto, nombreDestinatario, fechaCreacion, new List<PresupuestoDetalle>());
                        }

                        if (presupuestoDetalle != null)
                        {
                            presupuestoDict[idPresupuesto].Detalles.Add(presupuestoDetalle);
                        }
                    }

                    presupuestos = presupuestoDict.Values.ToList();
                }
                sqlitecon.Close();
            }
            return presupuestos;
        }



        public Presupuesto ObtenerPorId(int idPresupuesto)
        {
            var _cadenaDeConexion = "Data Source = db/Tienda.db";
            Presupuesto presupuesto = null;
            var consulta = @"
                SELECT Pres.idPresupuesto, Pres.NombreDestinatario, Pres.FechaCreacion, 
                    Prod.idProducto, Prod.Descripcion, Prod.Precio, PresD.Cantidad
                FROM Presupuestos Pres
                INNER JOIN PresupuestosDetalle PresD ON Pres.idPresupuesto = PresD.idPresupuesto
                INNER JOIN Productos Prod ON Prod.idProducto = PresD.idProducto
                WHERE Pres.idPresupuesto = @idPresupuesto";

            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

                using (var reader = command.ExecuteReader())
                {
                    List<PresupuestoDetalle> detalles = new List<PresupuestoDetalle>();

                    while (reader.Read())
                    {
                        if (presupuesto == null)
                        {
                            string nombreDestinatario = Convert.ToString(reader["NombreDestinatario"]);
                            DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                            presupuesto = new Presupuesto(idPresupuesto, nombreDestinatario, fechaCreacion, detalles);
                        }

                        int idProducto = Convert.ToInt32(reader["idProducto"]);
                        string descripcion = Convert.ToString(reader["Descripcion"]);
                        double precio = Convert.ToDouble(reader["Precio"]);
                        int cantidad = Convert.ToInt32(reader["Cantidad"]);

                        var producto = new Producto(idProducto, descripcion, precio);
                        var presupuestoDetalle = new PresupuestoDetalle(producto, cantidad);

                        detalles.Add(presupuestoDetalle);
                    }
                }
                sqlitecon.Close();
            }
            return presupuesto;
        }

    public bool AgregarProductoYcantidad(int idPresupuesto, Producto producto, int cantidad)
    {
        var _cadenaDeConexion = "Data Source = db/Tienda.db";
        bool resultado = false;

        var idProducto = producto.IdProducto;
        var consulta = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
                        VALUES (@idPresupuesto, @idProducto, @cantidad);";
        using (var sqliteConnection = new SqliteConnection(_cadenaDeConexion))
        {
            sqliteConnection.Open();
            using (var command = new SqliteCommand(consulta, sqliteConnection))
            {
                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                command.Parameters.AddWithValue("@idProducto", idProducto);
                command.Parameters.AddWithValue("@cantidad", cantidad);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 0)
                    resultado = true;
            }
            sqliteConnection.Close();
        }
        return resultado;
    }


    public bool Eliminar(int id)
    {
        var _cadenaDeConexion = "Data Source = db/Tienda.db";
        int rowsAffected;
        const string deleteDetalleQuery = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
        const string deletePresupuestoQuery = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";

        using (var sqliteConnection = new SqliteConnection(_cadenaDeConexion))
        {
            sqliteConnection.Open();

            using (var transaction = sqliteConnection.BeginTransaction())
            {
                try
                {
                    using (var detalleCommand = new SqliteCommand(deleteDetalleQuery, sqliteConnection, transaction))
                    {
                        detalleCommand.Parameters.AddWithValue("@id", id);
                        detalleCommand.ExecuteNonQuery();
                    }

                    using (var presupuestoCommand = new SqliteCommand(deletePresupuestoQuery, sqliteConnection, transaction))
                    {
                        presupuestoCommand.Parameters.AddWithValue("@id", id);
                        rowsAffected = presupuestoCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    sqliteConnection.Close();
                }
            }
        }
        return rowsAffected == 1;
    }


    // Método para editar un presupuesto existente
    public bool EditarPresupuesto(int idPresupuesto, string nuevoNombreDestinatario, DateTime nuevaFechaCreacion)
    {
        var _cadenaDeConexion = "Data Source = db/Tienda.db";
        if (string.IsNullOrEmpty(nuevoNombreDestinatario))
            throw new ArgumentException("El nombre del destinatario no puede estar vacío.", nameof(nuevoNombreDestinatario));

        bool resultado = false;

        var consulta = @"UPDATE Presupuestos 
                            SET NombreDestinatario = @nuevoNombreDestinatario, 
                                FechaCreacion = @nuevaFechaCreacion
                            WHERE IdPresupuesto = @idPresupuesto;";

        using (var sqliteConnection = new SqliteConnection(_cadenaDeConexion))
        {
            sqliteConnection.Open();

            using (var command = new SqliteCommand(consulta, sqliteConnection))
            {
                // Agregar parámetros a la consulta
                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                command.Parameters.AddWithValue("@nuevoNombreDestinatario", nuevoNombreDestinatario);
                command.Parameters.AddWithValue("@nuevaFechaCreacion", nuevaFechaCreacion);

                // Ejecutar la consulta
                int rowsAffected = command.ExecuteNonQuery();

                // Si se afectó alguna fila, el resultado es exitoso
                resultado = rowsAffected > 0;
            }

            sqliteConnection.Close();
        }

        return resultado;
    }
  }
}