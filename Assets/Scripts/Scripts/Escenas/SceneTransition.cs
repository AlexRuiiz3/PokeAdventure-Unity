using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string escenaSiguiente; //Es publica porque asi se sale en el inspector de unity y se puede poner el nombre de la escena manualmente

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collision.isTrigger)//Si entra en contacto con el jugador  
        {
            StartCoroutine(cargarEscena());
            //StopCoroutine(cargarEscena());
        }
    }

    IEnumerator cargarEscena()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        Vector2 posicionNuevaJugador = new Vector2(1f, 1f);

        switch (escenaSiguiente)
        {
            case "AdventureZone":
                string[] adventureScenes = { "SnowScene", "RouteScene", "ForestScene", "CityScene" };
                escenaSiguiente = "RouteScene";//adventureScenes[(int)(Random.Range(0f, 4f))]; //Numero entre 0 y 3

                switch (escenaSiguiente)
                {
                    case "SnowScene":
                        posicionNuevaJugador = new Vector2(-1.76f, -14.44f);
                        break;
                    case "RouteScene":
                        posicionNuevaJugador = new Vector2(-1.54f, -8.5f);
                        break;
                    case "ForestScene":
                        posicionNuevaJugador = new Vector2(0.6f, -21.12f);
                        break;
                    case "CityScene":
                        posicionNuevaJugador = new Vector2(-0.04f, -8.83f);
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
        }
        DontDestroyOnLoad(jugador);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(escenaSiguiente);
        jugador.transform.position = posicionNuevaJugador;
    }
}
