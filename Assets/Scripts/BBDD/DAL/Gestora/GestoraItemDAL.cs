using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GestoraItemDAL
{
    public static int actualizarItemsJugador(List<ItemConCantidad> items, int idJugador)
    {
        int inserciones = 0;
        SqliteConnection conexion = null;
        SqliteCommand command;
        if (items.Count > 0)
        {
            try
            {
                conexion = ConfiguracionDB.establecerConexion();
                string commandText = "DELETE FROM ItemsJugadores WHERE IDJugador = @IDJugador AND IDItem NOT IN (",
                    commandTextInserts = "";
                foreach (ItemConCantidad item in items)
                {
                    commandText += $"'{item.ID}',";
                    commandTextInserts += $"INSERT OR REPLACE INTO ItemsJugadores (IDItem, IDJugador,Cantidad) VALUES({item.ID},@IDJugador,{item.Cantidad}); ";
                }
                commandText = commandText.Remove(commandText.Length - 1);
                commandText += "); ";
                commandText += commandTextInserts;
                command = new SqliteCommand(commandText, conexion);
                command.Parameters.Add("@IDJugador", System.Data.DbType.UInt32).Value = idJugador;
                inserciones = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
                Debug.Log("Error en la obtencion de los datos de un item");
            }
            finally
            {
                ConfiguracionDB.cerrarConexion(conexion);
            }
        }
        return inserciones;
    }
}

