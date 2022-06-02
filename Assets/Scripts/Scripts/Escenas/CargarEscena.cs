using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    private GameObject jugador;
    private string escenaSiguiente;
    private void Start()
    {
        jugador = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        escenaSiguiente = PlayerPrefs.GetString("NameNextScene");
        StartCoroutine(cargarEscena());
    }

    IEnumerator cargarEscena() {
        jugador.SetActive(false);
        determinarPosicionJugador();

        AsyncOperation 
            operation = SceneManager.LoadSceneAsync(escenaSiguiente, LoadSceneMode.Single);
            operation.allowSceneActivation = false;
            yield return new WaitForSeconds(1f);
            operation.allowSceneActivation = true;
            operation.completed += (asyncOperation) =>{
                jugador.SetActive(true);
            };
            yield return operation;
    }
    private void determinarPosicionJugador() {

        bool modificarPosicionJugador = true;
        Vector2 posicionNuevaJugador = new Vector2(1f, 1f);
        switch (escenaSiguiente)
        {
            case "AdventureZone":
                string[] adventureScenes = { "SnowScene", "RouteScene", "ForestScene", "CityScene" };
                escenaSiguiente = adventureScenes[(int)Random.Range(0f, 4f)]; //Numero entre 0 y 3

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
                        posicionNuevaJugador = new Vector2(0.6f, -21.12f);
                        PlayerPrefs.SetString("EscenaAventura", "FondoBatallaMontaña");
                        break;
                    case "CityScene":
                        posicionNuevaJugador = new Vector2(-0.04f, -8.83f);
                        PlayerPrefs.SetString("EscenaAventura", "FondoBatallaHierba");
                        break;
                }
                break;

            case "PokemonCenterScene":
                posicionNuevaJugador = new Vector2(-2.9f, -1.2f);
                break;
            case "PokemonShopScene":
                posicionNuevaJugador = new Vector2(-7.52f, -5.09f);
                break;
            case "LobbyScene":
                switch (SceneManager.GetActiveScene().name)
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