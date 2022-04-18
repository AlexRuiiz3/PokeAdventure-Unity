using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosTipoDAL
{

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
            Debug.Log("Error en la obtencion de los tipos de un pokemon");
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
            Debug.Log("Error en la obtencion de los tipos debiles de un tipo");
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


