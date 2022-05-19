using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPC : MonoBehaviour
{
    public List<Button> botonesPokemonsEquipo;
    public GameObject plantillaButtonPokemonPC;
    private PokemonJugador pokemonSeleccionado;
    private Jugador jugador;

    private void OnEnable()
    {
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Total Pokemons: {jugador.EquipoPokemon.Count + DatosGuardarJugador.PokemonsAlmacenadosPC.Count}";

        configurarBotonesPokemonsEquipo();
        configurarBotonesPokemonPC();
    }

    public void verOpcionesPokemon(GameObject menuOpciones)
    {
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name;
        switch (EventSystem.current.currentSelectedGameObject.transform.parent.name)
        {

            case "ZonaPokemonsPC":
                pokemonSeleccionado = DatosGuardarJugador.PokemonsAlmacenadosPC.Find(g => g.PokemonNumero == Int16.Parse(nombreBoton));
                break;

            case "ZonaEquipoJugador":
                int identificadorBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);
                pokemonSeleccionado = jugador.EquipoPokemon[identificadorBotonPulsado - 1];
                break;

        }
        menuOpciones.SetActive(true);
    }

    private void configurarBotonesPokemonsEquipo()
    {

        for (int i = 0; i < jugador.EquipoPokemon.Count; i++)
        {
            botonesPokemonsEquipo[i].GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + jugador.EquipoPokemon[i].ID).First();
            botonesPokemonsEquipo[i].gameObject.SetActive(true);
        }
    }

    public void configurarBotonesPokemonPC()
    {
        try
        {
            GameObject content = gameObject.transform.Find("ZonaPokemonsPC").gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject,
                interfazPokemonPC;
            List<PokemonJugador> pokemonNoEquipados = DatosGuardarJugador.PokemonsAlmacenadosPC;
            foreach (PokemonJugador pokemon in pokemonNoEquipados)
            {
                interfazPokemonPC = Instantiate(plantillaButtonPokemonPC);
                interfazPokemonPC.name = pokemon.NumeroEquipado.ToString();
                interfazPokemonPC.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();

                interfazPokemonPC.transform.SetParent(content.transform);
                interfazPokemonPC.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
                interfazPokemonPC.SetActive(true);
            }
        }
        catch (Mono.Data.Sqlite.SqliteException)
        {
            UtilidadesEscena.mostrarMensajeError("Un error ocurrio mientras se obtenian los pokemons almacenados en el pc (No equipados)");
        }
        catch (Exception)
        {
            UtilidadesEscena.mostrarMensajeError("Error desconocido en la preparacion de los pokemons almacenados");
        }

    }


    public void liberarPokemon(GameObject menuOpcionesPokemon)
    {
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
            else
            {
                UtilidadesEscena.mostrarMensajeError("Debes tener minimo un pokemon equipado");
            }
        }
        else
        {  //Cuando sea en la zona de los pokemons del PC

        }
    }
}
