using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string nombreEscena; //Es publica porque asi se sale en el inspector de unity y se puede poner el nombre de la escena manualmente

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collision.isTrigger)
        { //Si colisiona con el jugador  
            if (nombreEscena.Equals("AdventureZone")) {
    
                string[] adventureScenes = {"SnowScene","RouteScene","ForestScene"};
                nombreEscena = adventureScenes[(int) (Random.Range(0f,3f))]; //Numero entre 0 y 2
            }
             SceneManager.LoadScene(nombreEscena);
            
        }
    }
}
