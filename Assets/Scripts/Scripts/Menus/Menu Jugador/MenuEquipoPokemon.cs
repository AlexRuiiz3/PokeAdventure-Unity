using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEquipoPokemon : MonoBehaviour
{
    public List<Button> interfacesPokemons;
    public List<Button> interfacesPokemonsMenuCambiarPosicion;
    private Jugador jugador;
    private GameObject interfazPokemonSeleccionado;
    private PokemonJugador pokemonSeleccionado;

    /// <summary>
    /// Cabecera: public void prepararMenuEquipo()
    /// Comentario: Este metodo se encarga configurar y activar el menu de equipo del jugador.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se mostrara el menu del equipo del jugador preparado con los pokemons que el jugador tenga en su equipo.
    /// </summary>
    public void prepararMenuEquipo() {
        Button interfazPokemon;
        PokemonJugador pokemon;
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;

        gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Equipo Actual {jugador.EquipoPokemon.Count}/6";
        UtilidadesEscena.activarDesactivarBotones(interfacesPokemons,false);
        for (int i = 0; i < jugador.EquipoPokemon.Count; i++)
        {
            pokemon = jugador.EquipoPokemon[i];
            interfazPokemon = interfacesPokemons[i];
            interfazPokemon.name = pokemon.PokemonNumero.ToString();
            interfazPokemon.GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
            UtilidadesEscena.modificarBarraSalud(interfazPokemon.GetComponentsInChildren<Image>()[3],pokemon.HP,pokemon.HPMaximos);
            interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemon.Nombre;
            interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"PS: {pokemon.HP} / {pokemon.HPMaximos}";
            interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Nvl.{pokemon.Nivel}";

            interfazPokemon.gameObject.SetActive(true);
        }
        if (jugador.EquipoPokemon.Count < 2)//Si el jugador tiene solo un pokemon, del menu de opciones del pokemon se desactiva el boton de cambiar posicion
        {
          Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuOpcionesPokemon").transform.Find("ButtonCambiarPosicion").gameObject.GetComponent<Button>().interactable = false;
        }
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public void mostrarConfigurarMenuOpcionesPokemon(GameObject interfazPokemon)
    /// Comentario: Este metodo se encarga de configurar y mostrar el menu con las opciones principales que se pueden realizar sobre un pokemon. 
    ///             Ademas guarda el pokemon que sea seleccionado y su interfaz.
    /// Entradas: GameObject interfazPokemon
    /// Salidas: Ninguna
    /// Precondiciones: interfazPokemon no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se mostrara un menu que cuenta con las principales operaciones que se realizan sobre un pokemon y se guadara el pokemon seleccionado y su interfaz.
    /// </summary>
    /// <param name="interfazPokemon"></param>
    public void mostrarConfigurarMenuOpcionesPokemon(GameObject interfazPokemon) {
        GameObject menuOpciones = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuOpcionesPokemon");
        interfazPokemonSeleccionado = interfazPokemon;
        pokemonSeleccionado = jugador.EquipoPokemon.Find(g => g.PokemonNumero == Int16.Parse(interfazPokemon.name));
        menuOpciones.GetComponentInChildren<TextMeshProUGUI>().text = pokemonSeleccionado.Nombre;
        menuOpciones.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public void opcionVerDatos(GameObject menuDatos)
    /// Comentario: Este metodo se encarga de recoger los datos necesarios para poder llamar al metodo configurarMenuDatosPokemon de la clase UtilidadesEscena.
    /// Entradas: GameObject menuDatos
    /// Salidas: Ninguna
    /// Precondiciones: menuDatos no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: El menu de datos esta configurado con los valores del pokemon seleccionado
    /// </summary>
    /// <param name="menuDatos"></param>
    public void opcionVerDatos(GameObject menuDatos) {
        UtilidadesEscena.configurarMenuDatosPokemon(menuDatos, pokemonSeleccionado);
    }

    /// <summary>
    /// Cabecera: public void buttonAceptarCambiarNombre(Text input)
    /// Comentario: Este metodo se encarga de cambiar el nombre a un pokemon seleccionado con el valor recibido por parametros. 
    /// Entradas: Text input
    /// Salidas: Ninguna
    /// Precondiciones: input no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se modificara el nombre del pokemon seleccionado. 
    ///                  Si el texto del valor recibido esta vacio el nombre no se modifica y se informa al jugador de ello.
    /// </summary>
    /// <param name="input"></param>
    public void buttonAceptarCambiarNombre(Text input) {
        if (!Utilidades.comprobarCadenaVacia(input.text))
        {
            interfazPokemonSeleccionado.GetComponentsInChildren<TextMeshProUGUI>()[0].text = input.text;
            GameObject menuOpciones = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuOpcionesPokemon");
            menuOpciones.GetComponentInChildren<TextMeshProUGUI>().text = input.text;
            
            pokemonSeleccionado.Nombre = input.text;
            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false); 
        }
        else {
            UtilidadesEscena.mostrarMensajeError("El nombre no puede estar vacio");
        }
    }

    /// <summary>
    /// Cabecera: public void mostrarConfigurarMenuCambiarPosicion()
    /// Comentario: Este metodo se encarga de configurar y activar el menu que permite al jugador intercambiar la posicion entre dos pokemons.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se mostrara un menu con botones por cada pokemon que tenga el jugador en su equipo.
    /// </summary>
    public void mostrarConfigurarMenuCambiarPosicion() {
        GameObject menuCambiarPosicion = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuCambiarPosicion");
        Button interfazPokemonCambiar;
        PokemonJugador pokemon;
        UtilidadesEscena.activarDesactivarBotones(interfacesPokemonsMenuCambiarPosicion, false);
        for (int i = 0; i < jugador.EquipoPokemon.Count; i++) {
            pokemon = jugador.EquipoPokemon[i];
            interfazPokemonCambiar = interfacesPokemonsMenuCambiarPosicion[i];
            interfazPokemonCambiar.name = pokemon.PokemonNumero.ToString();
            interfazPokemonCambiar.GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
            interfazPokemonCambiar.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemon.Nombre;
            interfazPokemonCambiar.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Nvl {pokemon.Nivel}";
            if (pokemon.Equals(pokemonSeleccionado)) {
                interfazPokemonCambiar.GetComponent<Button>().interactable = false;
            }
            interfazPokemonCambiar.gameObject.SetActive(true);
        }
        menuCambiarPosicion.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public void aplicarCambioPosicionPokemons(GameObject interfazPokemonCambiar)
    /// Comentario: Este metodo se encarga de cambiar las posicion entre dos pokemons del equipo del jugador. 
    /// Entradas: GameObject interfazPokemonCambiar
    /// Salidas: Ninguna
    /// Precondiciones: interfazPokemonCambiar no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se intercambiaran la posicion dos pokemons del equipo del jugador, las interfaces que representan a los pokemons tambien intercambiaran su posicion.
    /// </summary>
    /// <param name="interfazPokemonCambiar"></param>
    public void aplicarCambioPosicionPokemons(GameObject interfazPokemonCambiar) {
        PokemonJugador pokemonCambiar = jugador.EquipoPokemon.Find(g => g.PokemonNumero == Int16.Parse(interfazPokemonCambiar.name));

        int indexPokemonSeleccionado = jugador.EquipoPokemon.IndexOf(pokemonSeleccionado),
            indexPokemonCambiar = jugador.EquipoPokemon.IndexOf(pokemonCambiar),
            numeroEquipadoPokemonSeleccionado = pokemonSeleccionado.NumeroEquipado;

        //Los objetos pokemons intercambian sus atributos numeroEquipado
        pokemonSeleccionado.NumeroEquipado = pokemonCambiar.NumeroEquipado;
        pokemonCambiar.NumeroEquipado = numeroEquipadoPokemonSeleccionado;
        
        //Se intercambiar en la lista EquipoPokemon del jugador
        jugador.EquipoPokemon[indexPokemonSeleccionado] = pokemonCambiar;
        jugador.EquipoPokemon[indexPokemonCambiar] = pokemonSeleccionado;

        //Se realiza el intercambio de las interfaces de los pokemons
        GameObject contentPokemons = gameObject.transform.Find("ContentPokemons").gameObject,
                   interfazPokemonCambiarMenuEquipo = contentPokemons.transform.Find(pokemonCambiar.PokemonNumero.ToString()).gameObject;

        Vector3 positionPokemonSeleccionado = interfazPokemonSeleccionado.transform.position;
        interfazPokemonSeleccionado.transform.position = interfazPokemonCambiarMenuEquipo.transform.position;
        interfazPokemonCambiarMenuEquipo.transform.position = positionPokemonSeleccionado;

        Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuCambiarPosicion").SetActive(false);
    }
}
