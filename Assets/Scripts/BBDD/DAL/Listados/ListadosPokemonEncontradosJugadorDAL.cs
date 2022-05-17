using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosPokemonEncontradosJugadorDAL
{

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<PokemonEncontrado> obtenerPokemonsEncontradosDeJugador(string nombreUsuario)
    {
        List<PokemonEncontrado> pokemons = new List<PokemonEncontrado>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT PEJ.IDPokemon,PEJ.NombrePokemon FROM PokemonsEncontradosJugadores AS PEJ " +
                "INNER JOIN Jugadores AS J ON PEJ.IDJugador = J.ID " +
                "WHERE J.NombreUsuario = @NombreUsuario;", conexion);
            command.Parameters.Add("@NombreUsuario",System.Data.DbType.String).Value = nombreUsuario;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read()) {
                    pokemons.Add(new PokemonEncontrado(reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }
        catch (Exception)
        {
            Debug.Log("Error en la obtencion de los pokemons encontrados del jugador");
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return pokemons;
    }
}

