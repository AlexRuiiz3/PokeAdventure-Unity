﻿using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosPokemonEncontradosJugadorDAL
{
    /// <summary>
    /// Cabecera: public static List<PokemonEncontrado> obtenerPokemonsEncontradosDeJugador(int id)
    /// Comentario: Este metodo se encarga de obtener los pokemons que un jugador en especifico sea encontrado.
    /// Entradas: in id
    /// Salidas: List<PokemonEncontrado> pokemons 
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos PokemonEncontrado. Si se produce una excepcion, 
    ///                  el id no corresponde con el de ningun jugador o la consulta no tiene resultado, la lista devuelta estara vacia.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<PokemonEncontrado></returns>
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
        return pokemons;
    }
}
