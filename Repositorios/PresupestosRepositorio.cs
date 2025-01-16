using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios
{
    public class PresupuestoRepositorio : IPresupuestoRepositorio
    {
        public int CrearPresupuestoVacio(Presupuesto presupuesto)
        {
            var _cadenaDeConexion = "Data Source = db/Tienda.db";
            string NombreDestinatario = presupuesto.NombreDestinatario;
            DateTime FechaCreacion = presupuesto.FechaCreacion.Date;
            int idPresupuesto = 0;
            using (var sqlitecon = new SqliteConnection(_cadenaDeConexion))
            {
                sqlitecon.Open();

                var consultaPresupuesto = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                                            VALUES (@NombreDestinatario, @FechaCreacion);
                                            SELECT last_insert_rowid();";

                using (var commandPresupuesto = new SqliteCommand(consultaPresupuesto, sqlitecon))
                {
                    commandPresupuesto.Parameters.AddWithValue("@NombreDestinatario", NombreDestinatario);
                    commandPresupuesto.Parameters.AddWithValue("@FechaCreacion", FechaCreacion.Date);
                    idPresupuesto = Convert.ToInt32(commandPresupuesto.ExecuteScalar());
                }

                sqlitecon.Close();
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
            var consulta = @"SELECT Pres.idPresupuesto, Pres.NombreDestinatario, Pres.FechaCreacion, Prod.idProducto, Prod.Descripcion, Prod.Precio, PresD.Cantidad
                            FROM Presupuestos Pres
                            INNER JOIN  PresupuestosDetalle PresD ON Pres.idPresupuesto = PresD.idPresupuesto
                            INNER JOIN Productos Prod ON Prod.idProducto = PresD.idProducto";

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

                        int idProducto = Convert.ToInt32(reader["idProducto"]);
                        string descripcion = Convert.ToString(reader["Descripcion"]);
                        double precio = Convert.ToDouble(reader["Precio"]);
                        int cantidad = Convert.ToInt32(reader["Cantidad"]);

                        var producto = new Producto(idProducto, descripcion, precio);
                        var presupuestoDetalle = new PresupuestoDetalle(producto, cantidad);

                        // Verificar si el presupuesto ya está en el diccionario
                        if (!presupuestoDict.ContainsKey(idPresupuesto))
                        {
                            presupuestoDict[idPresupuesto] = new Presupuesto(idPresupuesto, nombreDestinatario, fechaCreacion, new List<PresupuestoDetalle>());
                        }

                        presupuestoDict[idPresupuesto].Detalles.Add(presupuestoDetalle);
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
        const string query = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
        using (var sqliteConnection = new SqliteConnection(_cadenaDeConexion))
        {
            sqliteConnection.Open();
            using (var command = new SqliteCommand(query, sqliteConnection))
            {
                command.Parameters.AddWithValue("@id", id);
                rowsAffected = command.ExecuteNonQuery();
                sqliteConnection.Close();
            }
            sqliteConnection.Close();
        }
        return rowsAffected==1;
    }
  }
}