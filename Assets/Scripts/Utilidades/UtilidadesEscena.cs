using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Clase con metodos que realizan alguna iteracion con los elementos de una escena, como activar o desactivar algo, 
 * o mostrar o eliminar algun elemento de una escena.
 */
public class UtilidadesEscena : MonoBehaviour
{

    private static bool menuActivado = false;
    public static void eliminarGameObjectsItemsYEntrenadores()
    {
        GameObject[] trainersYItems = GameObject.FindGameObjectsWithTag("Trainer");
        trainersYItems.Concat(GameObject.FindGameObjectsWithTag("Item"));

        foreach (GameObject gameObject in trainersYItems)
        {
            Destroy(gameObject);
        }
    }

    public static void activarDesactivarIteracionBotones(List<Button> botones, bool modo)
    {
        foreach (Button boton in botones)
        {
            boton.interactable = modo;
        }
    }

    public static void activarDesactivarBotones(List<Button> botones, bool modo)
    {
        foreach (Button boton in botones)
        {
            boton.gameObject.SetActive(modo);
        }
    }
    /// <summary>
    /// Cabecera: public static void activarDesactivarMenuYTiempoJuego(GameObject menu)
    /// Comentario: Este metodo se encarga de activar o desactivar un gameObject que sera un menu de interfaz de usuario. 
    ///             Tambien pausara el tiempo del juego, haciendo que el jugador no pueda moverse mientras el menu esta activo. 
    ///             Si el menu ya esta activo se desactiva y se vuelve ha activar el tiempo de juego.
    /// Entradas: GameObject menu
    /// Salidas: Niguna
    /// Precondiciones: menu tiene que ser distinto de null
    /// Postcondiciones: Se activa un gameObject que sera un menu y se parara el tiempo de juego. 
    ///                  Si ya esta activado el menu se hara lo contrario.
    ///                  Si el parametro menu esta a null, se producira un nullpointerException
    /// </summary>
    /// <param name="menu"></param>
    public static void activarDesactivarMenuYTiempoJuego(GameObject menu) {
        if (menuActivado)
        {
            Time.timeScale = 1f; //El tiempo del juego se reactiva
            menuActivado = false;
            menu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f; //El tiempo del juego pasa a ser 0 por que se pausa el juego 
            menuActivado = true;
            menu.SetActive(true);
        }
    }

    public static void pausarMusicaEscenaActiva()
    {
        bool encontrado = false;
        GameObject[] gameObjectsEscena = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < gameObjectsEscena.Length && !encontrado; i++)
        {
            if (gameObjectsEscena[i].name == "Grid")
            {
                encontrado = true;
                gameObjectsEscena[i].GetComponent<AudioSource>().Stop();
                gameObjectsEscena[i].GetComponent<AudioSource>().enabled = false;
                gameObjectsEscena[i].GetComponent<AudioListener>().enabled = false;
            }
        }
    }

    public static void activarMusicaEscenaActiva()
    {
        bool encontrado = false;
        GameObject[] gameObjectsEscena = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < gameObjectsEscena.Length && !encontrado; i++)
        {
            if (gameObjectsEscena[i].name == "Grid")
            {
                encontrado = true;
                gameObjectsEscena[i].GetComponent<AudioSource>().Play();
                gameObjectsEscena[i].GetComponent<AudioSource>().enabled = true;
                gameObjectsEscena[i].GetComponent<AudioListener>().enabled = true;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mensaje"></param>
    public static void mostrarMensajeError(string mensaje) 
    {
        GameObject menuError = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MensajeError");
        menuError.GetComponentsInChildren<TextMeshProUGUI>()[1].text = mensaje;
        menuError.SetActive(true);
    }

    public static void eliminarHijosGameObject(GameObject contentScrollView)
    {
        if (contentScrollView.transform.childCount > 0)
        {
            foreach (Transform child in contentScrollView.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}

