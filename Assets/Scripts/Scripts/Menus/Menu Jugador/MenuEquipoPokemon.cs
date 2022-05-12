using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuEquipoPokemon : MonoBehaviour
{
    public GameObject menuEquipo;
    private Jugador jugador;
    private GameObject interfazPokemonSeleccionado;
    private PokemonJugador pokemonSeleccionado;

    public void prepararMenuEquipo(GameObject plantillaInterfazPokemon) { 
        GameObject interfazPokemon = null, contentScrollView;
        PokemonJugador pokemon;
        menuEquipo = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuPokemons");
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        contentScrollView = menuEquipo.transform.Find("Content").gameObject;
        menuEquipo.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Equipo Actual {jugador.EquipoPokemon.Count}/6";
        for (int i = 0; i < 2; i++)//jugador.EquipoPokemon.Count
        {
            pokemon = jugador.EquipoPokemon[i];
            jugador.EquipoPokemon.Add(pokemon); //Eliminar
            pokemonSeleccionado = jugador.EquipoPokemon[i];
            interfazPokemon = Instantiate(plantillaInterfazPokemon);
            interfazPokemon.name = pokemon.ID.ToString();
            interfazPokemon.GetComponentsInChildren<Image>()[1].sprite = Utilidades.convertirArrayBytesASprite((pokemon.ImagenDeFrente != null) ? pokemon.ImagenDeFrente: pokemon.ImagenDeEspalda);
            interfazPokemon.GetComponentsInChildren<Image>()[2].transform.localScale = new Vector3((float)pokemon.HP / pokemon.HPMaximos, 1f, 1f);
            interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemon.Nombre;
            interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"PS: {pokemon.HP} / {pokemon.HPMaximos}";
            interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Nvl.{pokemon.Nivel}";

            interfazPokemon.transform.SetParent(contentScrollView.transform);
            interfazPokemon.transform.localScale = new Vector3(1,1,1);
            interfazPokemon.SetActive(true);
        }
        if (interfazPokemon != null && jugador.EquipoPokemon.Count < 2)
        {
            interfazPokemon.GetComponent<Button>().interactable = false;

        }
        menuEquipo.SetActive(true);
    }

    public void mostrarConfigurarMenuOpcionesPokemon(GameObject interfazPokemon) {
        GameObject menuOpciones = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuOpcionesPokemon");
        interfazPokemonSeleccionado = interfazPokemon;
        menuOpciones.GetComponentInChildren<TextMeshProUGUI>().text = pokemonSeleccionado.Nombre;
        menuOpciones.SetActive(true);
    }

    public void opcionVerDatos(GameObject menuDatos) {
        menuDatos.GetComponentsInChildren<Image>()[1].sprite = Utilidades.convertirArrayBytesASprite((pokemonSeleccionado.ImagenDeFrente != null) ? pokemonSeleccionado.ImagenDeFrente:pokemonSeleccionado.ImagenDeEspalda);
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemonSeleccionado.Nombre;
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Nvl.{pokemonSeleccionado.Nivel}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"PS: {pokemonSeleccionado.HP}/{pokemonSeleccionado.HPMaximos}";

        GameObject movimientos = menuDatos.transform.Find("Movimientos").gameObject, movimientoInterfaz;
        MovimientoPokemon movimientoPokemon;
        for (int i = 1; i < movimientos.transform.childCount; i++) { //Empieza en 1 porque ese hijo no es un movimiento sino un titulo
            movimientoPokemon = pokemonSeleccionado.Movimientos[i - 1];
            movimientoInterfaz = movimientos.transform.GetChild(i).gameObject;
            movimientoInterfaz.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/Banners/" + movimientoPokemon.Tipo)[0]; ;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[0].text = movimientoPokemon.Nombre;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Potencia: {movimientoPokemon.Danho}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Precicion: {movimientoPokemon.Precicion}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"PP {movimientoPokemon.PP}/{movimientoPokemon.PPMaximo}";
        }
        menuDatos.SetActive(true);
    }

    public void buttonAceptarCambiarNombre(Text input) {
        if (!Utilidades.comprobarCadenaVacia(input.text))
        {
            interfazPokemonSeleccionado.GetComponentsInChildren<TextMeshProUGUI>()[0].text = input.text;
            GameObject menuOpciones = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuOpcionesPokemon");
            menuOpciones.GetComponentInChildren<TextMeshProUGUI>().text = input.text;
            pokemonSeleccionado.Nombre = input.text;
            Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuCambiarNombre").SetActive(false);
        }
        else {
            UtilidadesEscena.mostrarMensajeError("El nombre no puede estar vacio");
        }
    }
}
