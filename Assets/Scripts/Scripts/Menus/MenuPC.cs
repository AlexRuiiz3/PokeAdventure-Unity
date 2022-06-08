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
    public List<GameObject> botonesPokemonsEquipo;
    public GameObject plantillaButtonPokemonPC;
    private PokemonJugador pokemonSeleccionado;
    private GameObject interfazPokemonSeleccionado;
    private Jugador jugador;

    //Cuando se active el menu
    private void OnEnable()
    {
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Total Pokemons: {jugador.EquipoPokemon.Count + DatosGuardarJugador.PokemonsAlmacenadosPC.Count}";

        configurarBotonesPokemonsEquipo();
        configurarBotonesPokemonPC();
    }
    //Cuando se desactive el menu
    private void OnDisable()
    {
        Time.timeScale = 1f;
        GameObject content = plantillaButtonPokemonPC.transform.parent.gameObject;
        UtilidadesEscena.eliminarHijosGameObject(content);
        UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/ClosePC", 1.5f);
    }

    //Metodo que configura y activa los botones del menu del equipo actual del jugador. El numero de botones que se activaran dependeran del numero de pokemon que el jugador tenga equipados.
    private void configurarBotonesPokemonsEquipo()
    {
        for (int i = 0; i < jugador.EquipoPokemon.Count; i++)
        {
            botonesPokemonsEquipo[i].GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + jugador.EquipoPokemon[i].ID).First();
            botonesPokemonsEquipo[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Cabecera: public void configurarBotonesPokemonPC()
    /// Comentario: Este metodo se encarga de configurar y activar los pokemons(Son botones) de la parte del menu de almacenamiento del PC. 
    ///             Los pokemons mostrados seran los que el jugador no tiene equipado en su equipo y el numero de botones que se crearan dependeran del 
    ///             numero de pokemons no equipados que tenga el jugador.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se crearan por cada pokemon que el jugador no tenga equipado un boton que representa un pokemon no equipado. 
    /// </summary>
    public void configurarBotonesPokemonPC()
    {
        GameObject content = plantillaButtonPokemonPC.transform.parent.gameObject,
            textSinPokemons = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "TextSinPokemonsAlmacenados");
        List<PokemonJugador> pokemonNoEquipados = DatosGuardarJugador.PokemonsAlmacenadosPC;

        if (pokemonNoEquipados.Count > 0)
        {
            textSinPokemons.SetActive(false);
            foreach (PokemonJugador pokemon in pokemonNoEquipados)
            {
                configurarMostrarPokemonPC(content, pokemon);
            }
        }
        else
        {
            textSinPokemons.SetActive(true);
        }
    }

    /// <summary>
    /// Cabecera: public void verOpcionesPokemon(GameObject menuOpciones)
    /// Comentario: Este metodo se encarga de obtener cual es el pokemon que sea seleccionado y mostrar el menu correspondiente en funcion de donde este el pokemon 
    ///             que se haya seleccionado.
    /// Entradas: GameObject menuOpciones
    /// Salidas: Ninguna
    /// Precondiciones: menuOpciones no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se obtiene el pokemon seleccionado y el menu correspondiente a la ubicacion del pokemon seleccionado. 
    ///                  Si el menu del pokemon de la zona opuesta esta activado, se desactiva.
    /// </summary>
    /// <param name="menuOpciones"></param>
    public void verOpcionesPokemon(GameObject menuOpciones)
    {
        GameObject menu;
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name;
        //Si el boton que se pulsa esta en la zona del equipo del jugador
        if (EventSystem.current.currentSelectedGameObject.transform.parent.name == "ZonaEquipoJugadorMenu")
        {
            //Si el menu de la zona del pc esta activado, se desactiva
            menu = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "MenuOpcionesPokemonPC");
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
            int identificadorBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);//nombreBoton en este caso, al final tienen un entero que indica que numero de boton es
            pokemonSeleccionado = jugador.EquipoPokemon[identificadorBotonPulsado - 1];
        }
        else //Sino si el boton que se pulsa esta en la zona del PC
        {
            //Si el menu de la zona del equoipo pokemon esta activado, se desactiva
            menu = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "MenuOpcionesPokemonEquipo");
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
            pokemonSeleccionado = DatosGuardarJugador.PokemonsAlmacenadosPC.Find(g => g.PokemonNumero == Int16.Parse(nombreBoton));//nombreBoton en este caso, al final tienen un entero unico que indica el numero de pokemon del jugador.
        }
        interfazPokemonSeleccionado = EventSystem.current.currentSelectedGameObject;
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
    public void opcionVerDatos(GameObject menuDatos)
    {
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
    public void buttonAceptarCambiarNombre(Text input)
    {
        if (!Utilidades.comprobarCadenaVacia(input.text))
        {
            pokemonSeleccionado.Nombre = input.text;
            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("El nombre no puede estar vacio");
        }
    }

    /// <summary>
    /// Cabecera: public void liberarPokemon(GameObject menuOpcionesPokemon)
    /// Comentario: Este metodo se encarga de eliminar para siempre el pokemon seleccionado del jugador.
    /// Entradas: GameObject menuOpcionesPokemon
    /// Salidas: Ninguna
    /// Precondiciones: menuOpcionesPokemon no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se eliminara un pokemon del juagador. Dependiendo de la zona donde este el pokemon, se eliminara del equipo actual del jugador o del almacenamiento del PC
    ///                  El pokemon no se eliminara en los siguientes casos:
    ///                  1: Si el pokemon a eliminar esta en la zona de equipo actual de jugador y es el ultimo que tiene el jugador en su equipo, no se elimina y se informa al jugador de ello. 
    ///                  2: Si se produce alguna excepcion el pokemon no se eliminara.
    /// </summary>
    /// <param name="menuOpcionesPokemon"></param>
    public void liberarPokemon(GameObject menuOpcionesPokemon)
    {
        string nombreZona = menuOpcionesPokemon.transform.parent.name;
        if (nombreZona == "ZonaEquipoJugadorMenu") //Cuando sea en la zona del equipo del jugador
        {
            if (jugador.EquipoPokemon.Count > 1)
            {
                jugador.EquipoPokemon.Remove(pokemonSeleccionado);
                UtilidadesEscena.activarDesactivarGameObjects(botonesPokemonsEquipo, false);//Se desactivan todos
                configurarBotonesPokemonsEquipo();
                menuOpcionesPokemon.SetActive(false);
            }
            else
            {
                UtilidadesEscena.mostrarMensajeError("Debes tener minimo un pokemon equipado");
            }
        }
        else //Cuando sea en la zona de los pokemons del PC
        {
            eliminarPokemonAlmacenado();
            menuOpcionesPokemon.SetActive(false);
        }
        menuOpcionesPokemon.transform.Find("Menus").transform.Find("MenuLiberarPokemon").gameObject.SetActive(false);
    }

    /// <summary>
    /// Cabecera: public void equiparPokemon()
    /// Comentario: Este metodo se encarga de incluir en el equipo actual de jugador un pokemon seleccionado.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se añadira en el equipo actual del jugador el pokemon seleccionado. El pokemon no se añadira al equipo de jugador en los siguientes casos:
    ///                  1: Si el numero de pokemons que tiene el jugador en su equipo es 6, el pokemon no se añadira y se informara al jugador de ello. 
    ///                  2: Si se produce alguna excepcion el pokemon no se añadira.
    /// </summary>
    public void equiparPokemon()
    {
        if (jugador.EquipoPokemon.Count < 6)
        {
            //Se quita la interfaz del pokemon del menu de almacenamiento
            eliminarPokemonAlmacenado();

            pokemonSeleccionado.NumeroEquipado = jugador.EquipoPokemon.Count + 1;
            //Se añade la interfaz del pokemon al menu equipo 
            jugador.EquipoPokemon.Add(pokemonSeleccionado);
            UtilidadesEscena.activarDesactivarGameObjects(botonesPokemonsEquipo, false);//Se desactivan todos
            configurarBotonesPokemonsEquipo();

            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);//Se desactiva el menu del pokemon seleccionado
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("El equipo ya esta completo. Deja antes un pokemon");
        }
    }

    /// <summary>
    /// Cabecera: public void dejarPokemon() 
    /// Comentario: Este metodo se encarga de quitar del equipo actual de jugador un pokemon seleccionado.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se quitara del equipo actual del jugador el pokemon seleccionado. El pokemon no se quitara del equipo de jugador en los siguientes casos:
    ///                  1: Si el pokemon que se quiere dejar es el ultimo que tiene el jugador en su equipo, el pokemon no se quitara y se informara al jugador de ello. 
    ///                  2: Si se produce alguna excepcion el pokemon no se quitara.
    /// </summary>
    public void dejarPokemon()
    {
        if (jugador.EquipoPokemon.Count > 1)
        {
            //Se quita la interfaz del pokemon del menu equipo 
            jugador.EquipoPokemon.Remove(pokemonSeleccionado);
            UtilidadesEscena.activarDesactivarGameObjects(botonesPokemonsEquipo, false);//Se desactivan todos
            configurarBotonesPokemonsEquipo();
            for (int i = 0; i < jugador.EquipoPokemon.Count; i++)
            {
                jugador.EquipoPokemon[i].NumeroEquipado = i + 1;
            }
            pokemonSeleccionado.NumeroEquipado = 0;
            //Se añade la intefaz del pokemon al menu de almacenamiento
            DatosGuardarJugador.PokemonsAlmacenadosPC.Add(pokemonSeleccionado);
            GameObject content = plantillaButtonPokemonPC.transform.parent.gameObject;
            Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "TextSinPokemonsAlmacenados").SetActive(false);
            configurarMostrarPokemonPC(content, pokemonSeleccionado);

            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("Debes tener minimo un pokemon equipado");
        }
    }

    //Metodo que se encarga de crear, configurar y activar un boton(Pokemon) ubicado en la zona de los pokemons almacenados en el PC(Los no equipados)  
    private void configurarMostrarPokemonPC(GameObject content, PokemonJugador pokemon)
    {
        GameObject interfazPokemonPC = Instantiate(plantillaButtonPokemonPC);
        interfazPokemonPC.name = pokemon.PokemonNumero.ToString();
        interfazPokemonPC.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();

        interfazPokemonPC.transform.SetParent(content.transform);
        interfazPokemonPC.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        interfazPokemonPC.SetActive(true);
    }

    private void eliminarPokemonAlmacenado()
    {
        DatosGuardarJugador.PokemonsAlmacenadosPC.Remove(pokemonSeleccionado);
        Destroy(interfazPokemonSeleccionado);
        if (DatosGuardarJugador.PokemonsAlmacenadosPC.Count < 1)
        {
            Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "TextSinPokemonsAlmacenados").SetActive(true);
        }
    }
}
