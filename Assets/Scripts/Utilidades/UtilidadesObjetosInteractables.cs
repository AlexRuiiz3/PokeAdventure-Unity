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
        switch (PlayerPrefs.GetString("InteraccionConObjeto")) {
            case "Medico": curarPokemonsJugador(); break;
            case "Vendedor": activarMenuObjecto("MenuVendedor"); break;
            case "PC": activarMenuObjecto("MenuPC"); break;
            case "Trainer": /*activarMenuObjecto("MenuPC");*/ break;
        }
    }

    private static void curarPokemonsJugador() {
        Jugador jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;

        foreach (PokemonJugador pokemon in jugador.EquipoPokemon) {
            pokemon.HP = pokemon.HPMaximos;
        }
    }

    private static void activarMenuObjecto(string nombreMenu) {
        GameObject menu = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == nombreMenu);
        UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menu);
    }
}
