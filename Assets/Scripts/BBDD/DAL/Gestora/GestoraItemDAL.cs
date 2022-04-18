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

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("DELETE FROM ItemsJugadores WHERE IDJugador = @IDJugador;",conexion);
            command.Parameters.Add("@IDJugador",System.Data.DbType.UInt32).Value = idJugador;

            foreach (ItemConCantidad item in items) {
                if (item.Cantidad > 0) {
                    command.Parameters.Add("@IDItem", System.Data.DbType.UInt32).Value = item.ID;
                    command.Parameters.Add("@Cantidad", System.Data.DbType.UInt32).Value = item.Cantidad;
                    command.CommandText += "INSERT INTO ItemsJugadores VALUES(@IDItem,@IDJugador,@Cantidad);";
                }
            }
            inserciones = command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Debug.Log("Error en la obtencion de los datos de un item");
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
        }
        return inserciones;
    }
}

