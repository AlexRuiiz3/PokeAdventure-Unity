using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJugador : MonoBehaviour
{
    public GameObject menu;
    private bool menuActivado;

    private void Start()
    {
        menuActivado = false;
        menu.SetActive(false);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
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
    }
    public void verPokemons() {
        Debug.Log("Ver pokemons");
    }

    public void verPokedex()
    {
        Debug.Log("Ver pokedex");
    }

    public void verMochila()
    {
        Debug.Log("Ver mochila");
    }

    public void guardarPartida()
    {
        Debug.Log("Guardar");
    }

    public void configuracion(GameObject menuConfiguracion)
    {
        menuConfiguracion.SetActive(true);
    }

    public void verPerfil(GameObject menuPerfil)
    {
        menuPerfil.SetActive(true);
    }
    public void salir()
    {
        //EditorUtility.DisplayDialog("Datos incorrectos", "Vas a volver a la pantalla principal, los cambios no guardados se perderan", "Ok");
        SceneManager.LoadScene("MainScene");
    }
}
