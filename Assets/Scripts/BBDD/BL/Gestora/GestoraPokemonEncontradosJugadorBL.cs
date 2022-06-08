using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoraPokemonEncontradosJugadorBL
{
    public static void insertarPokemonsEncontradosAJugador(int idJugador, List<PokemonEncontrado> pokemonsEncontrados) 
    {
        GestoraPokemonEncontradosJugadorDAL.insertarPokemonsEncontradosAJugador(idJugador, pokemonsEncontrados);
    }
}
