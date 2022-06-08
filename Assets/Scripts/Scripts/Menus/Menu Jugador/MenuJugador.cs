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

    public void guardarPartida()
    {
        try
        {
            Jugador jugador = GetComponent<PlayerController>().Jugador;

            List<PokemonJugador> listaPokemonsCompleta = new List<PokemonJugador>(DatosGuardarJugador.PokemonsAlmacenadosPC);
            jugador.EquipoPokemon.ForEach(p => listaPokemonsCompleta.Add(p));
            GestoraPokemonsJugadorBL.guardarPokemonsDeJugador(jugador.ID,listaPokemonsCompleta);
            GestoraItemDAL.actualizarItemsJugador(jugador.Mochila,jugador.ID);
            GestoraPokemonEncontradosJugadorBL.insertarPokemonsEncontradosAJugador(jugador.ID,DatosGuardarJugador.PokemonsEncontradosJugador);
            GestoraJugadorBL.actualizarDineroJugador(jugador.ID,jugador.Dinero);

            UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/SaveGame", 1.5f);
            StartCoroutine(mostrarGuardadoConExito());
            GameObject.Find("MenuGuardar").SetActive(false);
        }
        catch (Exception)
        {
            UtilidadesEscena.mostrarMensajeError("Ocurrio un error realizando el guardado de la partida");
        }
    }
    public void cerrarMenu()
    { 
        UtilidadesEscena.cerrarMenus(menuJugador); 
        menuJugador.SetActive(true);
        UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menuJugador);
    }
    public void salir()
    {
        UtilidadesEscena.cerrarMenus(menuJugador);
        UtilidadesEscena.eliminarGameObjectsItemsYEntrenadores();
        Time.timeScale = 1f;
        UtilidadesEscena.precargarEscena("MainScene");
    }

    public void prepararMostrarMenuPerfil(GameObject menuPerfil)
    {
        Jugador jugador = GetComponent<PlayerController>().Jugador;

        menuPerfil.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Nombre: {jugador.NombreUsuario}";
        menuPerfil.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"Dinero: {jugador.Dinero}$";
        menuPerfil.GetComponentsInChildren<TextMeshProUGUI>()[4].text = $"Correo Electronico: {jugador.CorreoElectronico}";
        menuPerfil.SetActive(true);
    }

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
