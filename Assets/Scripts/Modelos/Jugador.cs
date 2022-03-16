using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : ClsJugador
{
    public Jugador(ClsJugador jugador, List<PokemonJugador> equipoPokemon, List<ItemConCantidad> mochila ) : base(jugador.ID, jugador.NombreUsuario, jugador.Contrasenha, jugador.Contrasenha, jugador.NivelCuenta, jugador.Experiencia, jugador.Dinero, jugador.Foto) {
        EquipoPokemon = equipoPokemon;
        Mochila = mochila;
    }
    public List<PokemonJugador> EquipoPokemon { get; set; }
    public List<ItemConCantidad> Mochila { get; set; }
}
