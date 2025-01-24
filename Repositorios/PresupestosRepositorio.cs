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
            if (presupuesto.Cliente == null)
            {
                throw new ArgumentException("El presupuesto debe estar asociado a un cliente.", nameof(presupuesto.Cliente));
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

                    // Consulta para insertar el presupuesto
                    var consultaPresupuesto = @"
                        INSERT INTO Presupuestos (ClienteId, FechaCreacion) 
                        VALUES (@ClienteId, @FechaCreacion);
                        SELECT last_insert_rowid();";

                    using (var commandPresupuesto = new SqliteCommand(consultaPresupuesto, sqlitecon))
                    {
                        // Usar ClienteId en lugar de NombreDestinatario
                        commandPresupuesto.Parameters.AddWithValue("@ClienteId", presupuesto.Cliente.ClienteId);
                        commandPresupuesto.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);

                        // Ejecutar la consulta y obtener el id del presupuesto insertado
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
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            var presupuestos = new List<Presupuesto>();
            var consulta = @"SELECT Pres.idPresupuesto, Pres.FechaCreacion, 
                                    Cli.ClienteId, Cli.Nombre AS NombreCliente, Cli.Email, Cli.Telefono
                            FROM Presupuestos Pres
                            INNER JOIN Clientes Cli ON Pres.ClienteId = Cli.ClienteId";

            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                using (var command = new SqliteCommand(consulta, sqlitecon))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Datos del presupuesto
                            int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                            DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]).Date;

                            // Datos del cliente
                            int clienteId = Convert.ToInt32(reader["ClienteId"]);
                            string nombreCliente = Convert.ToString(reader["NombreCliente"]);
                            string email = Convert.ToString(reader["Email"]);
                            string telefono = Convert.ToString(reader["Telefono"]);

                            // Crear el objeto Cliente
                            Cliente cliente = new Cliente(clienteId, nombreCliente, email, telefono);

                            // Crear el objeto Presupuesto
                            var presupuesto = new Presupuesto(idPresupuesto, cliente, fechaCreacion, new List<PresupuestoDetalle>());

                            // Agregar el presupuesto a la lista
                            presupuestos.Add(presupuesto);
                        }
                    }
                }
                sqlitecon.Close();
            }
            return presupuestos;
        }



        public Presupuesto ObtenerTablaPresupuestosPorId(int id)
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            Presupuesto presupuesto = null;

            // Nueva consulta para incluir ClienteId y obtener datos del cliente relacionado
            var consulta = @"SELECT p.idPresupuesto, p.ClienteId, p.FechaCreacion, 
                                    c.Nombre AS NombreCliente, c.Email, c.Telefono
                            FROM Presupuestos p
                            INNER JOIN Clientes c ON p.ClienteId = c.ClienteId
                            WHERE p.idPresupuesto = @id";

            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                using (var command = new SqliteCommand(consulta, sqlitecon))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Datos del presupuesto
                            int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                            DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]).Date;

                            // Datos del cliente relacionado
                            int clienteId = Convert.ToInt32(reader["ClienteId"]);
                            string nombreCliente = Convert.ToString(reader["NombreCliente"]);
                            string email = Convert.ToString(reader["Email"]);
                            string telefono = Convert.ToString(reader["Telefono"]);

                            // Crear instancia de Cliente
                            Cliente cliente = new Cliente(clienteId, nombreCliente, email, telefono);

                            // Crear instancia de Presupuesto con el cliente
                            presupuesto = new Presupuesto(idPresupuesto, cliente, fechaCreacion, new List<PresupuestoDetalle>());
                        }
                    }
                }
                sqlitecon.Close();
            }
            return presupuesto;
        }

                    
            


        public List<Presupuesto> ObtenerPresupuestoCompleto()
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            var presupuestos = new List<Presupuesto>();
            var consulta = @"SELECT Pres.idPresupuesto, Pres.ClienteId, Pres.FechaCreacion,
                                    Prod.idProducto, Prod.Descripcion, Prod.Precio, PresD.Cantidad,
                                    Cli.Nombre AS NombreCliente, Cli.Email, Cli.Telefono
                            FROM Presupuestos Pres
                            LEFT JOIN PresupuestosDetalle PresD ON Pres.idPresupuesto = PresD.idPresupuesto
                            LEFT JOIN Productos Prod ON Prod.idProducto = PresD.idProducto
                            INNER JOIN Clientes Cli ON Pres.ClienteId = Cli.ClienteId";

            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();
                using (var command = new SqliteCommand(consulta, sqlitecon))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var presupuestoDict = new Dictionary<int, Presupuesto>();

                        while (reader.Read())
                        {
                            // Datos del presupuesto
                            int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                            DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]).Date;

                            // Datos del cliente
                            int clienteId = Convert.ToInt32(reader["ClienteId"]);
                            string nombreCliente = Convert.ToString(reader["NombreCliente"]);
                            string email = Convert.ToString(reader["Email"]);
                            string telefono = Convert.ToString(reader["Telefono"]);
                            // Crear instancia de Cliente
                            Cliente cliente = new Cliente(clienteId, nombreCliente, email, telefono);

                            // Datos del producto y detalle
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
                                presupuestoDict[idPresupuesto] = new Presupuesto(idPresupuesto, cliente,fechaCreacion , new List<PresupuestoDetalle>());
                            }

                            // Agregar el detalle si existe
                            if (presupuestoDetalle != null)
                            {
                                presupuestoDict[idPresupuesto].Detalles.Add(presupuestoDetalle);
                            }
                        }

                        presupuestos = presupuestoDict.Values.ToList();
                    }
                }
                sqlitecon.Close();
            }
            return presupuestos;
        }


public Presupuesto ObtenerPorId(int idPresupuesto)
{
    var _cadenaDeConexion = "Data Source=db/Tienda.db";
    Presupuesto presupuesto = null;

    var consulta = @"
        SELECT Pres.idPresupuesto, Pres.FechaCreacion, 
               Cli.ClienteId, Cli.Nombre AS NombreCliente, Cli.Email, Cli.Telefono,
               Prod.idProducto, Prod.Descripcion, Prod.Precio, PresD.Cantidad
        FROM Presupuestos Pres
        LEFT JOIN Clientes Cli ON Pres.ClienteId = Cli.ClienteId
        LEFT JOIN PresupuestosDetalle PresD ON Pres.idPresupuesto = PresD.idPresupuesto
        LEFT JOIN Productos Prod ON Prod.idProducto = PresD.idProducto
        WHERE Pres.idPresupuesto = @idPresupuesto";

    using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
    {
        sqlitecon.Open();
        using (var command = new SqliteCommand(consulta, sqlitecon))
        {
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            using (var reader = command.ExecuteReader())
            {
                List<PresupuestoDetalle> detalles = new List<PresupuestoDetalle>();

                while (reader.Read())
                {
                    if (presupuesto == null)
                    {
                        // Datos del cliente
                        int? clienteId = reader["ClienteId"] != DBNull.Value ? Convert.ToInt32(reader["ClienteId"]) : (int?)null;
                        Cliente cliente = null;
                        if (clienteId.HasValue)
                        {
                            string nombreCliente = Convert.ToString(reader["NombreCliente"]);
                            string email = Convert.ToString(reader["Email"]);
                            string telefono = Convert.ToString(reader["Telefono"]);
                            cliente = new Cliente(clienteId.Value, nombreCliente, email, telefono);

                        }

                        // Datos del presupuesto
                        DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                        presupuesto = new Presupuesto(idPresupuesto, cliente, fechaCreacion, detalles);
                    }

                    // Datos de los detalles del presupuesto
                    if (reader["idProducto"] != DBNull.Value)
                    {
                        int idProducto = Convert.ToInt32(reader["idProducto"]);
                        string descripcion = Convert.ToString(reader["Descripcion"]);
                        double precio = Convert.ToDouble(reader["Precio"]);
                        int cantidad = Convert.ToInt32(reader["Cantidad"]);

                        var producto = new Producto(idPresupuesto, descripcion, precio);

                        var presupuestoDetalle = new PresupuestoDetalle(producto, cantidad);

                        detalles.Add(presupuestoDetalle);
                    }
                }
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
        // Método para editar un presupuesto existente
    public bool EditarPresupuesto(int idPresupuesto, Cliente nuevoCliente, DateTime nuevaFechaCreacion)
    {
        var _cadenaDeConexion = "Data Source=db/Tienda.db";

        // Validar que el cliente no sea nulo
        if (nuevoCliente == null || nuevoCliente.ClienteId <= 0)
            throw new ArgumentException("El cliente especificado no es válido.", nameof(nuevoCliente));

        bool resultado = false;

        var consulta = @"UPDATE Presupuestos 
                        SET ClienteId = @clienteId, 
                            FechaCreacion = @nuevaFechaCreacion
                        WHERE IdPresupuesto = @idPresupuesto;";

        using (var sqliteConnection = new SqliteConnection(_cadenaDeConexion))
        {
            sqliteConnection.Open();

            using (var command = new SqliteCommand(consulta, sqliteConnection))
            {
                // Agregar parámetros a la consulta
                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                command.Parameters.AddWithValue("@clienteId", nuevoCliente.ClienteId);
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