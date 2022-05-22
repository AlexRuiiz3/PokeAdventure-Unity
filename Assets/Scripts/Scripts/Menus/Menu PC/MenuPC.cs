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
    private GameObject interfazPokemonSeleccionado;
    private Jugador jugador;

    private void OnEnable()
    {

        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Total Pokemons: {jugador.EquipoPokemon.Count + DatosGuardarJugador.PokemonsAlmacenadosPC.Count}";

        configurarBotonesPokemonsEquipo();
        configurarBotonesPokemonPC();
    }

    private void OnDisable()
    {
        GameObject content = plantillaButtonPokemonPC.transform.parent.gameObject;
        UtilidadesEscena.eliminarHijosGameObject(content);
    }
    public void verOpcionesPokemon(GameObject menuOpciones)
    {
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name;
        if (EventSystem.current.currentSelectedGameObject.transform.parent.name == "ZonaEquipoJugador")
        {
            int identificadorBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);
            pokemonSeleccionado = jugador.EquipoPokemon[identificadorBotonPulsado - 1]; 
        }
        else
        {
            pokemonSeleccionado = DatosGuardarJugador.PokemonsAlmacenadosPC.Find(g => g.PokemonNumero == Int16.Parse(nombreBoton));
        }
        interfazPokemonSeleccionado = EventSystem.current.currentSelectedGameObject;
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
            GameObject content = plantillaButtonPokemonPC.transform.parent.gameObject;
            List<PokemonJugador> pokemonNoEquipados = DatosGuardarJugador.PokemonsAlmacenadosPC;
            foreach (PokemonJugador pokemon in pokemonNoEquipados)
            {
                configurarMostrarPokemonPC(content,pokemon);
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

    public void opcionVerDatos(GameObject menuDatos)
    {
        menuDatos.GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemonSeleccionado.ID).First();
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemonSeleccionado.Nombre;
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Nvl.{pokemonSeleccionado.Nivel}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"PS: {pokemonSeleccionado.HP}/{pokemonSeleccionado.HPMaximos}";

        GameObject movimientos = menuDatos.transform.Find("Movimientos").gameObject, movimientoInterfaz;
        MovimientoPokemon movimientoPokemon;
        for (int i = 1; i < movimientos.transform.childCount; i++)
        { //Empieza en 1 porque ese hijo no es un movimiento sino un titulo
            movimientoPokemon = pokemonSeleccionado.Movimientos[i - 1];
            movimientoInterfaz = movimientos.transform.GetChild(i).gameObject;
            movimientoInterfaz.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/Banners/" + movimientoPokemon.Tipo)[0]; ;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[0].text = movimientoPokemon.Nombre;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Potencia: {movimientoPokemon.Danho}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Presicion: {movimientoPokemon.Precicion}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"PP {movimientoPokemon.PP}/{movimientoPokemon.PPMaximo}";
        }
        menuDatos.SetActive(true);
    }

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
            DatosGuardarJugador.PokemonsAlmacenadosPC.Remove(pokemonSeleccionado);
            Destroy(interfazPokemonSeleccionado);
            menuOpcionesPokemon.SetActive(false);
        }
        menuOpcionesPokemon.transform.Find("Menus").transform.Find("MenuLiberarPokemon").gameObject.SetActive(false);
    }

    public void equiparPokemon() {
        if (jugador.EquipoPokemon.Count < 6)
        {
            //Se quita la interfaz del pokemon del menu de almacenamiento
            DatosGuardarJugador.PokemonsAlmacenadosPC.Remove(pokemonSeleccionado);
            Destroy(interfazPokemonSeleccionado);

            //Se añade la interfaz del pokemon al menu equipo 
            jugador.EquipoPokemon.Add(pokemonSeleccionado);
            UtilidadesEscena.activarDesactivarBotones(botonesPokemonsEquipo, false);//Se desactivan todos
            configurarBotonesPokemonsEquipo();

            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        }
        else {
            UtilidadesEscena.mostrarMensajeError("El equipo ya esta completo. Deja antes un pokemon");
        }
    }

    public void dejarPokemon() {
        if (jugador.EquipoPokemon.Count > 1)
        {
            //Se quita la interfaz del pokemon del menu equipo 
            jugador.EquipoPokemon.Remove(pokemonSeleccionado);
            UtilidadesEscena.activarDesactivarBotones(botonesPokemonsEquipo, false);//Se desactivan todos
            configurarBotonesPokemonsEquipo();

            //Se añade la intefaz del pokemon al menu de almacenamiento
            DatosGuardarJugador.PokemonsAlmacenadosPC.Add(pokemonSeleccionado);
            GameObject content = plantillaButtonPokemonPC.transform.parent.gameObject;
            configurarMostrarPokemonPC(content, pokemonSeleccionado);

            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("Debes tener minimo un pokemon equipado");
        }
    }

    private void configurarMostrarPokemonPC(GameObject content, PokemonJugador pokemon) {
        GameObject interfazPokemonPC = Instantiate(plantillaButtonPokemonPC);
        interfazPokemonPC.name = pokemon.PokemonNumero.ToString();
        interfazPokemonPC.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();

        interfazPokemonPC.transform.SetParent(content.transform);
        interfazPokemonPC.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        interfazPokemonPC.SetActive(true);
    }
}
