using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class CargarEscena : MonoBehaviour
{
    private GameObject jugador;
    private bool jugadorMirandoArriba;
    private string escenaSiguiente;
    
    private void Start()
    {
        jugador = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        escenaSiguiente = PlayerPrefs.GetString("NameNextScene");
        StartCoroutine(cargarEscena());
    }

    //Este metodo se encarga de cargar una escena en segundo plano y cuando este lista activarla 
    IEnumerator cargarEscena() {
        jugador.SetActive(false);
        bool escenasTitulo = true;
        float segundosEspera = 2f;
        if (escenaSiguiente != "GetFirstPokemonScene" && escenaSiguiente != "MainScene")
        {
            determinarPosicionJugador();
            escenasTitulo = false;
            segundosEspera = Random.Range(3, 7);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(escenaSiguiente, LoadSceneMode.Single);
        operation.allowSceneActivation = false;
            yield return new WaitForSeconds(segundosEspera);
            operation.allowSceneActivation = true;
            operation.completed += (asyncOperation) =>{
                if (!escenasTitulo) {
                    jugador.SetActive(true);
                    if (jugadorMirandoArriba) { //Para que funcione el cambio de animacion el jugador tiene que estar activado
                        Animator animatorPlayer = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player")).GetComponent<PlayerController>().GetComponent<Animator>();
                        animatorPlayer.SetFloat("moveY", 0.1f);
                        animatorPlayer.SetBool("isMoving", true);
                    }
                }
            };
            yield return operation;
    }
    
    //Metodo que se encarga de en funcion de la escena a la que vaya el jugador determinar que posicion inicial debe tener en esa escena
    private void determinarPosicionJugador() {
        bool modificarPosicionJugador = true;
        Vector2 posicionNuevaJugador = new Vector2(1f, 1f);
        switch (escenaSiguiente)
        {
            case "AdventureZone":
                string[] adventureScenes = { "SnowScene", "RouteScene", "ForestScene", "CityScene" };
                escenaSiguiente = adventureScenes[(int)Random.Range(0f, 4f)]; //Numero entre 0 y 3
                jugadorMirandoArriba = true;
                switch (escenaSiguiente)
                {
                    case "SnowScene":
                        posicionNuevaJugador = new Vector2(-1.76f, -14.44f);
                        PlayerPrefs.SetString("EscenaAventura", "FondoBatallaNieve");
                        break;
                    case "RouteScene":
                        posicionNuevaJugador = new Vector2(-1.54f, -8.5f);
                        PlayerPrefs.SetString("EscenaAventura", "FondoBatallaHierba");
                        break;
                    case "ForestScene":
                        posicionNuevaJugador = new Vector2(0.5f, -19.18f);
                        PlayerPrefs.SetString("EscenaAventura", "FondoBatallaMontaña");
                        break;
                    case "CityScene":
                        posicionNuevaJugador = new Vector2(-0.04f, -8.83f);
                        PlayerPrefs.SetString("EscenaAventura", "FondoBatallaHierba");
                        break;
                }
                break;

            case "PokemonCenterScene":
                if (PlayerPrefs.GetString("NameLastScene").Contains("Battle"))
                {
                    UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/Recovery", 3f);
                    posicionNuevaJugador = new Vector2(-3f, 3.5f);
                }
                else {
                    posicionNuevaJugador = new Vector2(-2.9f, -1.2f);
                }
                jugadorMirandoArriba = true;
                break;
            case "PokemonShopScene":
                jugadorMirandoArriba = true;
                posicionNuevaJugador = new Vector2(-7.52f, -5.09f);
                break;
            case "LobbyScene":
                switch (PlayerPrefs.GetString("NameLastScene"))
                {
                    case "SnowScene":
                    case "RouteScene":
                    case "ForestScene":
                    case "CityScene":
                        posicionNuevaJugador = new Vector2(-1, 5);
                        UtilidadesEscena.eliminarGameObjectsItemsYEntrenadores();
                        break;

                    case "PokemonCenterScene":
                        posicionNuevaJugador = new Vector2(4.5f, -1f);
                        break;

                    case "PokemonShopScene":
                        posicionNuevaJugador = new Vector2(-6.84f, -1f);
                        break;
                    default:
                        posicionNuevaJugador = new Vector2(-1f, -1f);
                        break;
                }
                break;

            default:
                modificarPosicionJugador = false;
                break;
        }
        if (modificarPosicionJugador)
        {
            jugador.transform.position = posicionNuevaJugador;
        }
    }
}
