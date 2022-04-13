using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPC : MonoBehaviour
{
    public GameObject menu;
    public List<Button> botonesPokemonsEquipo;
    private PokemonJugador pokemonSeleccionado;
    private Jugador jugador;

    private void Start()
    {
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        
        configurarBotonesPokemonsEquipo();
    }

    private void Update()
    {

    }

    public void verOpcionesPokemon(GameObject menuOpciones)
    {
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name;
        int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);
        pokemonSeleccionado = jugador.EquipoPokemon[numeroBotonPulsado - 1];
        menuOpciones.SetActive(true);
    }

    private void configurarBotonesPokemonsEquipo() 
    {
        byte[] imagenPokemon;
        for (int i = 0; i < jugador.EquipoPokemon.Count; i++) {
            //Hay pokemons que no tienen imagen de frente, entonces se coge la imagen de espalda
            imagenPokemon = (jugador.EquipoPokemon[i].ImagenDeFrente != null) ? jugador.EquipoPokemon[i].ImagenDeFrente : jugador.EquipoPokemon[i].ImagenDeEspalda;
            botonesPokemonsEquipo[i].gameObject.SetActive(true);
            botonesPokemonsEquipo[i].GetComponent<Image>().sprite = Utilidades.convertirArrayBytesASprite(imagenPokemon);
        }
    }

    public void liberarPokemon() {
        GameObject menuOpcionesPokemon = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        string nombreZona = menuOpcionesPokemon.transform.parent.name;
        if (nombreZona == "ZonaEquipoJugador")
        {
            if (jugador.EquipoPokemon.Count > 1)
            {

                //Si el boton pulsado es que esta en la zona
                jugador.EquipoPokemon.Remove(pokemonSeleccionado);
                UtilidadesEscena.activarDesactivarBotones(botonesPokemonsEquipo, false);//Se desactivan todos
                configurarBotonesPokemonsEquipo();
                menuOpcionesPokemon.SetActive(false);
            }
            else {
                Debug.Log("Tienes que tener minimo un pokemon");
            }
        }
        else { 
        //Aqui cuando sea la zona del pc
        }
    }
}
