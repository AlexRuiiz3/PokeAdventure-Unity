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
    public static List<PokemonEncontrado> obtenerPokemonsEncontradosDeJugador(int id)
    {
        List<PokemonEncontrado> pokemons = new List<PokemonEncontrado>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT IDPokemon, NombrePokemon FROM PokemonsEncontradosJugadores " +
                "WHERE IDJugador = @ID;", conexion);
            command.Parameters.Add("@ID", System.Data.DbType.UInt32).Value = id;
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

    public static int obtenerNumeroPokemonsEncontradosDeJugador(int id)
    {
        int pokemonsEncontrados = 0;
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT COUNT(*) FROM PokemonsEncontradosJugadores " +
                                        "WHERE IDJugador = @ID " +
                                        "GROUP BY IDJugador;", conexion);
            command.Parameters.Add("@ID", System.Data.DbType.UInt32).Value = id;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                pokemonsEncontrados = reader.GetInt32(0);
            }
        }
        catch (Exception)
        {
            Debug.Log("Error en la obtencion del numero de pokemons encontrados del jugador");
            throw;
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return pokemonsEncontrados;
    }
}

