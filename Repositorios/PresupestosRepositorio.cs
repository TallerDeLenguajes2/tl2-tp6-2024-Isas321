using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiWebAPI.Models;
using Microsoft.Data.Sqlite;

namespace MiWebAPI.Repositorios{
  public class PresupuestosRepositorio : IPresupuestosRepositorio
  {
    public void Crear(Presupuesto presupuesto)
    {
        var NombreDestinatario = Convert.ToString(presupuesto.NombreDestinatario);
        var FechaCreacion = Convert.ToDateTime(presupuesto.FechaCreacion);

        var cadena = "Data Source=db/Tienda.db";

        using (var sqlitecon = new SqliteConnection(cadena))
        {
            sqlitecon.Open();
            var consulta = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                            VALUES (@NombreDestinatario, @FechaCreacion);";

            using (var comand = new SqliteCommand(consulta, sqlitecon))
            {
                comand.Parameters.AddWithValue("@NombreDestinatario", NombreDestinatario);
                comand.Parameters.AddWithValue("@FechaCreacion", FechaCreacion);

                comand.ExecuteNonQuery();
            }
        }
    }



    public List<Presupuesto> ObtenerTodos(){

      List<Presupuesto> presupuestos = new List<Presupuesto>(); 
      var cadena = "Data Source = db/Tienda.db";
      using( var sqlitecon = new SqliteConnection(cadena)){
        sqlitecon.Open();      
        var consulta = @"SELECT * FROM Presupuestos";
        SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
          var id = Convert.ToInt32(reader["idPresupuesto"]);
          var NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]);
          var FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
          Presupuesto presupuesto = new Presupuesto(id, NombreDestinatario, FechaCreacion);
          presupuestos.Add(presupuesto);
        }
        sqlitecon.Close();
      }
      return presupuestos;
    }


    public Presupuesto ObtenerPresupuestoPorId(int id)
    {
      int idPresupuesto = 0;
      string NombreDestinatario = "Sin destinatario";
      DateTime FechaCreacion = DateTime.MinValue;

      var cadena = "Data Source = db/Tienda.db";
      using (var sqlitecon = new SqliteConnection(cadena))
      {
          sqlitecon.Open();
          var consulta = @"SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
          SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
          command.Parameters.AddWithValue("@id", id);
          var reader = command.ExecuteReader();

          if (reader.Read())
          {
              idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
              NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]);
              FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
          }

          sqlitecon.Close();
      }
      return new Presupuesto(idPresupuesto, NombreDestinatario, FechaCreacion);
    }

    public bool Actualizar(int idPresupuesto, Producto producto, int cantidad)
    {
        if (idPresupuesto <= 0)
        {
            return false;
        }

        var idProducto = producto.IdProducto;

        var connectionString = "Data Source=db/Tienda.db";
        using (var sqliteConnection = new SqliteConnection(connectionString))
        {
            sqliteConnection.Open();
            const string query = "UPDATE PresupuestosDetalle SET Cantidad = @cantidad WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto;";
            
            using (var command = new SqliteCommand(query, sqliteConnection))
            {
                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                command.Parameters.AddWithValue("@idProducto", idProducto);
                command.Parameters.AddWithValue("@cantidad", cantidad);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool Eliminar(int id)
    {
        int rowsAffected;
        var connectionString = "Data Source=db/Tienda.db";
        using (var sqliteConnection = new SqliteConnection(connectionString))
        {
            const string query = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
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
  }
}