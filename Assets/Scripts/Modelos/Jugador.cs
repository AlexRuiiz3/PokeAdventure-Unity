/*
 * Clase: Jugador
 * 
 * Comentario: Esta clase representa a un jugador dentro de una partida que cuenta con un equipo pokemon y una mochila de items. Hereda de la clase ClsJugador.
 * 
 * Atributos:
 *          Basicos:
 *              equipoPokemon: Listado, Consultable.
 *              mochila: Listado, Consultable.
 *          Derivados: Ninguno.
 *          Compartidos: Ninguno.
 *          
 * Metodos fundamentales(Propiedades)
 *              EquipoPokemon: public List<PokemonJugador> EquipoPokemon { get; }
 *              Mochila: public List<ItemConCantidad> Mochila { get; }
 *
 * Metodos añadidos: Ninguno.
 * 
 * Metodos heredados: Ninguno.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : ClsJugador
{
    #region Constructores
    //Constructor con parametros
    public Jugador(ClsJugador jugador, List<PokemonJugador> equipoPokemon, List<ItemConCantidad> mochila ) : base(jugador.ID, jugador.NombreUsuario, jugador.Contrasenha, jugador.Contrasenha, jugador.Dinero, jugador.Foto) {
        EquipoPokemon = equipoPokemon;
        Mochila = mochila;
    }
    #endregion
    
    #region Metodos Fundamentales(Propiedades)
    public List<PokemonJugador> EquipoPokemon { get; }
    public List<ItemConCantidad> Mochila { get; }
    #endregion
}
