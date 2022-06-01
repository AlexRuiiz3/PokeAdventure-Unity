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
            PlayerPrefs.SetString("NameLastScene", SceneManager.GetActiveScene().name);
            UtilidadesEscena.precargarEscena(escenaSiguiente);
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
}
