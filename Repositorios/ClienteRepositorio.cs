using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios
{
public class ClienteRepositorio : IClienteRepositorio
{
        public List<Cliente> ObtenerTodos()
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            var clientes = new List<Cliente>();

            using (var connection = new SqliteConnection(_cadenaDeConexion))
            {
                connection.Open();
                var query = "SELECT * FROM Clientes";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ClienteId = reader.GetInt32(0);
                        var Nombre = reader.GetString(1);
                        var Email = reader.GetString(2);
                        var Telefono = reader.GetString(3);

                        var cliente = new Cliente(ClienteId, Nombre, Email, Telefono);
                        clientes.Add(cliente);
                    }
                }
            }
            return clientes;
        }

        public Cliente ObtenerPorId(int id)
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            Cliente cliente = null; // Se inicializa en null

            using (var connection = new SqliteConnection(_cadenaDeConexion))
            {
                connection.Open();
                var query = "SELECT ClienteId, Nombre, Email, Telefono FROM Clientes WHERE ClienteId = @id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Asignar a la variable cliente declarada fuera del bloque
                                var ClienteId = reader.GetInt32(0);
                                var Nombre = reader.GetString(1);
                                var Email = reader.GetString(2);
                                var Telefono = reader.GetString(3);

                                cliente = new Cliente(ClienteId, Nombre, Email, Telefono);
                        }
                    }
                }
            }
            return cliente; // Devolver el cliente o null si no se encontró
        }


        public bool Crear(Cliente cliente)
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            using (var connection = new SqliteConnection(_cadenaDeConexion))
            {
                connection.Open();
                var query = "INSERT INTO Clientes (Nombre, Email, Telefono) VALUES (@nombre, @email, @telefono)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@email", cliente.Email);
                    command.Parameters.AddWithValue("@telefono", cliente.Telefono);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


        public bool Actualizar(Cliente cliente)
        {
            var _cadenaDeConexion = "Data Source=db/Tienda.db";
            using (var connection = new SqliteConnection(_cadenaDeConexion))
            {
                connection.Open();
                var query = "UPDATE Clientes SET Nombre = @nombre, Email = @email, Telefono = @telefono WHERE ClienteId = @id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@email", cliente.Email);
                    command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@id", cliente.ClienteId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                int rowsAffected;
                var connectionString = "Data Source=db/Tienda.db";
                using (var connection = new SqliteConnection(connectionString))
                {
                    const string query = "DELETE FROM Clientes WHERE ClienteId = @id";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@id", id);
                        rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;  // Si se eliminó al menos un registro
                    }
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // Error 19: Clave foránea
            {
                // Log o mensaje de error en caso de violación de clave foránea
                Console.WriteLine("No se puede eliminar el cliente porque está relacionado con otros registros.");
                return false;
            }
        }

    }
}
