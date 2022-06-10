using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoraPokemonEncontradosJugadorBL
{
    /// <summary>
    /// Cabecera: public static void insertarPokemonsEncontradosAJugador(int idJugador, List<PokemonEncontrado> pokemonsEncontrados) 
    /// Comentario: Este metodo se encarga de llamar al metodo insertarPokemonsEncontradosAJugador de la clase GestoraPokemonEncontradosJugadorDAL de la capa DAL.
    /// Entradas: int idJugador, List<PokemonEncontrado> pokemonsEncontrados
    /// Salidas: void
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se insertara en la base de datos los pokemons encontrados asociados a un jugador.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dinero"></param>
    /// <returns>int</returns>
    public static void insertarPokemonsEncontradosAJugador(int idJugador, List<PokemonEncontrado> pokemonsEncontrados) 
    {
        GestoraPokemonEncontradosJugadorDAL.insertarPokemonsEncontradosAJugador(idJugador, pokemonsEncontrados);
    }
}
