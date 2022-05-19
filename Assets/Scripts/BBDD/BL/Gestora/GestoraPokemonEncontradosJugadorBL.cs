using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoraPokemonEncontradosJugadorBL
{
    public static void insertarPokemonEncontradoAJugador(int idJugador, int idPokemon, string nombrePokemon) 
    {
        GestoraPokemonEncontradosJugadorDAL.insertarPokemonEncontradoAJugador(idJugador, idPokemon, nombrePokemon);
    }
}
