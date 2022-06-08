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
    public void determinarAccionFinDialogo() {// No es static porque sino no se podria usar StartCoroutine
        string objetoInteraccion = PlayerPrefs.GetString("InteraccionConObjeto");

        switch (objetoInteraccion) {
            case "Medico":
                UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/Recovery",3f);
                curarPokemonsJugador(); break;
            case "Vendedor": activarMenuObjeto("MenuTienda"); break;
            case "PC": activarMenuObjeto("MenuPC"); break;
            case "Objeto":
                UtilidadesEscena.destruirGameObjectEspecifico("AudioTemporal");
                UtilidadesEscena.activarPausarMusicaEscenaActiva(true);
                PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();
                playerController.iniciarCoroutineAsignarObjetoEncontrado();
                //StartCoroutine(playerController.asignarObjetoEncontrado()); //No se puede hacer aqui ya que esta clase no esta asociada a ningun gameObject del juego
                break;
            case "TrainerInteracion":
                UtilidadesEscena.destruirGameObjectEspecifico("AudioTemporal");
                PlayerPrefs.SetString("NameNextScene", "BattleTrainerScene");
                SceneManager.LoadScene("BattleTrainerScene", LoadSceneMode.Additive);
                break;
        }
    }

    private static void curarPokemonsJugador() {
        Jugador jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;

        foreach (PokemonJugador pokemon in jugador.EquipoPokemon) {
            pokemon.HP = pokemon.HPMaximos;
        }
    }
    private static void activarMenuObjeto(string nombreMenu) {
        GameObject menu = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == nombreMenu);
        UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menu);
    }
}
