using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GestoraItemDAL
{
    /// <summary>
    /// Cabecera: public static int eliminarYActualizarItemsJugador(List<ItemConCantidad> items, int idJugador)
    /// Comentario: Este metodo se encarga de eliminar de la base de datos los items que no se encuentren en una lista de items(Mochila, lista de items del jugador) y 
    ///             añadir los items que no existan en la base de datos y si en la lista y actualizar aquellos que si existan.
    /// Entradas: int idJugador, List<PokemonEncontrado> pokemonsEncontrados
    /// Salidas: int inserciones
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran operaciones de manipulacion(Insert,Delete,Update) sobre los items de un jugador en la base de datos. 
    ///                  No se realizara ninguna operacion en los siguientes casos:
    ///                  1: Si el id del jugador no existe en la base de datos o no es valido, no se realiara ninguna operacion. 
    ///                  2: Si la lista recibida esta vacia, no hay items                        
    /// </summary>
    /// <param name="items"></param>
    /// <param name="idJugador"></param>
    /// <returns>int</returns>
    public static int eliminarYActualizarItemsJugador(List<ItemConCantidad> items, int idJugador)
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

