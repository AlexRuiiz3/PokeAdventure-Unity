using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosMovimientoDAL
{

    /// <summary>
    /// Cabecera: public static List<MovimientoPokemon> obtenerMovimientosPokemon(int idJugador, int idPokemon, int numeroPokemon)
    /// Comentario: Este metodo se encarga de obtener de la base de datos los movimientos que tiene asociados un pokemon. 
    /// Entradas: int idJugador, int idPokemon, int numeroPokemon
    /// Salidas: List<MovimientoPokemon> movimientos
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo MovmientosPokemon, si se produce alguna excepcion o no se encuentran resultados en la consulta, 
    ///                  la lista devuelta estara vacia.
    /// </summary>
    /// <param name="idJugador"></param>
    /// <param name="idPokemon"></param>
    /// <param name="numeroPokemon"></param>
    /// <returns>List<MovimientoPokemon></returns>
    public static List<MovimientoPokemon> obtenerMovimientosPokemon(int idJugador, int idPokemon, int numeroPokemon) {
        List<MovimientoPokemon> movimientos = new List<MovimientoPokemon>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT M.MT, M.Nombre, M.Danho, M.Precision, M.PP, " +
                "(SELECT Nombre FROM Tipos WHERE ID = M.Tipo) FROM Movimientos AS M " +
                "INNER JOIN PokemonsJugadoresMovimientos AS PJM ON M.MT = PJM.IDMovimiento " +
                "WHERE PJM.IDJugador = @IDJugador AND PJM.IDPokemon = @IDPokemon AND PJM.NumeroPokemon = @NumeroPokemon;", conexion);
            command.Parameters.Add("@IDJugador", System.Data.DbType.Int32).Value = idJugador;
            command.Parameters.Add("@IDPokemon", System.Data.DbType.Int32).Value = idPokemon;
            command.Parameters.Add("@NumeroPokemon", System.Data.DbType.Int32).Value = numeroPokemon;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read()) {
                    movimientos.Add(new MovimientoPokemon(reader.GetInt32(0),reader.GetString(1), 
                                    reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5))
                        );
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return movimientos; 
    }
}
