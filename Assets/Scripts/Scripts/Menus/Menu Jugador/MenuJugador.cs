using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJugador : MonoBehaviour
{
    public GameObject menuJugador;
    
    private void Update()
    {
        if (PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString() && Input.GetKeyDown(KeyCode.C))
        {
            if (menuJugador.activeSelf)
            {
                UtilidadesEscena.cerrarMenus(menuJugador);
                menuJugador.SetActive(true);
            }
            UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menuJugador);
        }
    }

    /// <summary>
    /// Cabecera: public void guardarPartida()
    /// Comentario: Este metodo se encarga de realizar las llamadas a los metodos necesarios para guardar todos los datos de un jugador en la base de datos.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se guardaran en la base de datos todos los datos asociados a un jugador. Si se produce alguna expcecion, algunos datos no se guardaran.
    /// </summary>
    public void guardarPartida()
    {
        try
        {
            Jugador jugador = GetComponent<PlayerController>().Jugador;

            List<PokemonJugador> listaPokemonsCompleta = new List<PokemonJugador>(DatosGuardarJugador.PokemonsAlmacenadosPC);
            jugador.EquipoPokemon.ForEach(p => listaPokemonsCompleta.Add(p));
            GestoraPokemonsJugadorBL.guardarPokemonsDeJugador(jugador.ID,listaPokemonsCompleta);
            GestoraItemDAL.eliminarYActualizarItemsJugador(jugador.Mochila,jugador.ID);
            GestoraPokemonEncontradosJugadorBL.insertarPokemonsEncontradosAJugador(jugador.ID,DatosGuardarJugador.PokemonsEncontradosJugador);
            GestoraJugadorBL.actualizarDineroJugador(jugador.ID,jugador.Dinero);

            UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/SaveGame", 1.5f);
            StartCoroutine(mostrarGuardadoConExito());
            GameObject.Find("MenuGuardar").SetActive(false);
        }
        catch (Exception)
        {
            throw;
            UtilidadesEscena.mostrarMensajeError("Ocurrio un error realizando el guardado de la partida");
        }
    }
    /// <summary>
    /// Cabecera: public void cerrarMenu()
    /// Comentario: Este metodo se encarga de cerrar el menu principal del jugador.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: menuJugador no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se desactivara el menu principal del jugador.
    /// </summary>
    public void cerrarMenu()
    { 
        UtilidadesEscena.cerrarMenus(menuJugador); 
        menuJugador.SetActive(true);
        UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menuJugador);
    }
    
    /// <summary>
    /// Cabecera: public void salir()
    /// Comentario: Este metodo se encarga de salir de la partida actual del jugador.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se saldra de la escena en la que se encuentre el jugador y se cargara la escena MainScene.
    /// </summary>
    public void salir()
    {
        UtilidadesEscena.cerrarMenus(menuJugador);
        UtilidadesEscena.eliminarGameObjectsItemsYEntrenadores();
        Time.timeScale = 1f;
        UtilidadesEscena.precargarEscena("MainScene");
    }

    /// <summary>
    /// Cabecera: public void prepararMostrarMenuPerfil(GameObject menuPerfil)
    /// Comentario: Este metodo se encarga de configurar y mostrar el menu de perfil del jugador.
    /// Entradas: GameObject menu
    /// Salidas: Ninguna
    /// Precondiciones: menuPerfil no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se activara configurado el menu de perfil del jugador 
    /// <param name="menuPerfil"></param>
    /// </summary>
    public void prepararMostrarMenuPerfil(GameObject menuPerfil)
    {
        Jugador jugador = GetComponent<PlayerController>().Jugador;

        menuPerfil.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Nombre: {jugador.NombreUsuario}";
        menuPerfil.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"Dinero: {jugador.Dinero}$";
        menuPerfil.GetComponentsInChildren<TextMeshProUGUI>()[4].text = $"Correo Electronico: {jugador.CorreoElectronico}";
        menuPerfil.SetActive(true);
    }

    /// <summary>
    /// Cabecera: IEnumerator mostrarGuardadoConExito()
    /// Comentario: Esta corrutina se encarga de mostrar un mensaje de guardado con exito temporalmente durante unos segundos.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se activara un mensaje que se desactivara a los pocos segundos.
    /// </summary>
    IEnumerator mostrarGuardadoConExito()
    {
        GameObject menuTemporal = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MensajeTemporal");
        menuTemporal.GetComponentInChildren<TextMeshProUGUI>().text = "Guardado realizado con exito";
        menuTemporal.SetActive(true);
        yield return new WaitForSecondsRealtime(1.25f);
        menuTemporal.SetActive(false);
        StopCoroutine(mostrarGuardadoConExito());
    }
}
