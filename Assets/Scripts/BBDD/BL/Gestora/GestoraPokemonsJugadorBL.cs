using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

public class GestoraPokemonsJugadorBL
{
    public static void guardarPokemonsDeJugador(int idJugador, List<PokemonJugador> pokemons)
    {
        GestoraPokemonsJugadorDAL.guardarPokemonsDeJugador(idJugador,pokemons);
    }

}

