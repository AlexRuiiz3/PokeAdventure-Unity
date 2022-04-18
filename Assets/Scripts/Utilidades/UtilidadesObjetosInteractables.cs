using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UtilidadesObjetosInteractables
{
    public static void determinarAccionFinDialogo() {
        string objetoInteraccion = PlayerPrefs.GetString("InteraccionConObjeto");
        switch (objetoInteraccion) {
            case "Medico": curarPokemonsJugador(); break;
            case "Vendedor": activarMenuObjecto("MenuVendedor"); break;
            case "PC": activarMenuObjecto("MenuPC"); break;
            case "Pocion": case "Pokeball": asignarObjetoAJugador(objetoInteraccion); break;
            case "Trainer": /*activarMenuObjecto("MenuPC");*/ break;
            
        }
    }

    private static void curarPokemonsJugador() {
        Jugador jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;

        foreach (PokemonJugador pokemon in jugador.EquipoPokemon) {
            pokemon.HP = pokemon.HPMaximos;
        }
    }

    private static void asignarObjetoAJugador(string nombreObjeto) {
        Item item = new Item();
        
        /*
        switch (nombreObjeto.Contains()) {
            case "Pokeball": item = new Item(1, nombreObjeto, "Dispositivo que sirve para capturar pokemons", 70, 0, "Pokeball"); break;
            case "Pocion": item = new Item(1, nombreObjeto, "Cura una pequeña cantidad de 50hp a un pokemon", 0, 50, "Pocion"); break;
        }
        */
        GameObject.Find("Player").GetComponent<PlayerController>().Jugador.Mochila.Add(new ItemConCantidad(item,1));
    }

    private static void activarMenuObjecto(string nombreMenu) {
        GameObject menu = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == nombreMenu);
        UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menu);
    }
}
