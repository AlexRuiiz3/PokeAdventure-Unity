using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

public class GestoraPokemonsJugadorBL
{
    /// <summary>
    /// Cabecera: public static void guardarPokemonsDeJugador(int idJugador, List<PokemonJugador> pokemons) 
    /// Comentario: Este metodo se encarga de llamar al metodo guardarPokemonsDeJugador de la clase GestoraPokemonsJugadorDAL de la capa DAL.
    /// Entradas: int idJugador, List<PokemonJugador> pokemons
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se insertaran, eliminaran o actualizaran los pokemons asociados a un jugador en la base de datos.
    /// </summary>
    /// <param name="idJugador"></param>
    /// <param name="pokemons"></param>
    /// <returns></returns>
    public static void guardarPokemonsDeJugador(int idJugador, List<PokemonJugador> pokemons)
    {
        GestoraPokemonsJugadorDAL.guardarPokemonsDeJugador(idJugador,pokemons);
    }
}

