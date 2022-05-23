using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosTipoDAL
{
    /// <summary>
    /// Cabecera: public static List<string> obtenerTiposPokemon(int idPokemon)
    /// Comentario: Este metodo se encarga de obtener los tipos que tiene un pokemon especifico de la base de datos.
    /// Entradas: int idPokemon
    /// Salidas: List<string> tipos
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un lista de string que contendra los tipos de un pokemon en especifico. Si se produce una excepcion o no se encuentra un pokemon 
    //                   con el id recibido o la consulta no tiene resultados, la lista devuelta estara vacia.
    /// </summary>
    /// <param name="idPokemon"></param>
    /// <returns>List<string></returns>
    public static List<string> obtenerTiposPokemon(int idPokemon)
    {
        List<string> tipos = new List<string>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT T.Nombre FROM Tipos AS T " +
                "INNER JOIN TiposPokemons AS TP ON T.ID = TP.IDTipo " +
                "WHERE TP.IDPokemon = @IDPokemon;", conexion);
            command.Parameters.Add("@IDPokemon", System.Data.DbType.Int32).Value = idPokemon;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tipos.Add(reader.GetString(0));
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
        return tipos;
    }

    /// <summary>
    /// Cabecera: public static List<string> obtenerTiposDebilesTipo(string tipo)
    /// Comentario: Este metodo se encarga de obtener los tipos frente a los que es debil un pokemon especifico de la base de datos.
    /// Entradas: string tipo
    /// Salidas: List<string> tiposDebiles
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un lista de string que contendra los tipos debiles de un pokemon en especifico. Si se produce una excepcion o no se encuentra un pokemon 
    //                   con el id recibido o la consulta no tiene resultados, la lista devuelta estara vacia.
    /// </summary>
    /// <param name="tipo"></param>
    /// <returns>List<string></returns>
    public static List<string> obtenerTiposDebilesTipo(string tipo)
    {
        List<string> tiposDebiles = new List<string>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT (SELECT Nombre FROM Tipos WHERE ID = TTD.IDTipoDebil) FROM Tipos AS T " +
                "INNER JOIN TiposTiposDebiles AS TTD ON T.ID = TTD.IDTipo WHERE T.Nombre = @Tipo ;", conexion);
            command.Parameters.Add("@Tipo", System.Data.DbType.String).Value = tipo;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tiposDebiles.Add(reader.GetString(0));
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
        return tiposDebiles;
    }
}
