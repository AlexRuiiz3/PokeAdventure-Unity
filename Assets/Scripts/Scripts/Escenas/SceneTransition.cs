using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string escenaSiguiente; //Es publica porque asi se sale en el inspector de unity y se puede poner el nombre de la escena manualmente
    private bool jugadorDentro;
    private GameObject canvasMensajeAyudaJugador;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    /// 

    private void Start()
    {
        canvasMensajeAyudaJugador = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasMensajeAyuda");
    }
    private void Update()
    {
        if (jugadorDentro && Input.GetKey(KeyCode.R))
        {
            canvasMensajeAyudaJugador.SetActive(false);
            StartCoroutine(cargarEscena());
            //StopCoroutine(cargarEscena());
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collision.isTrigger)//Si entra en contacto con el jugador  
        {
            canvasMensajeAyudaJugador.SetActive(true);
            jugadorDentro = true;
        }
    }
    //Metodo para controlar cuando el jugador sale de la zona de iteracion que tiene asociado un objeto que es interactable
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvasMensajeAyudaJugador.SetActive(false);
        jugadorDentro = false;
    }
    IEnumerator cargarEscena()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
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
        PlayerPrefs.SetString("NameLastScene",SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(escenaSiguiente);
        jugador.transform.position = posicionNuevaJugador;
    }
}
