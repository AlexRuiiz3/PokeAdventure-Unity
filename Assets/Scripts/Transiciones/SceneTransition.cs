using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string escenaActual; //Es publica porque asi se sale en el inspector de unity y se puede poner el nombre de la escena manualmente
    public string escenaSiguiente; //Es publica porque asi se sale en el inspector de unity y se puede poner el nombre de la escena manualmente

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collision.isTrigger)
        { //Si colisiona con el jugador  
            DatosGenerales.ultimaEscena = escenaActual;
 
            if (escenaSiguiente.Equals("AdventureZone")) {
                
                string[] adventureScenes = {"SnowScene","RouteScene","ForestScene","CityScene"};
                escenaSiguiente = adventureScenes[(int) (Random.Range(0f,4f))]; //Numero entre 0 y 3
            }
             SceneManager.LoadScene(escenaSiguiente);  
        }
    }
}
