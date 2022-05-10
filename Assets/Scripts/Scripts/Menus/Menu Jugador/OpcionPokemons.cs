using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpcionPokemons : MonoBehaviour
{
    public PlayerController scriptPlayer;//Creo que tiene que ser GameObject
    public List<Button> botonesPokemons;
    public GameObject menuOpcionesPokemon;
    public TextMeshProUGUI textNumeroPokemons;
    private Jugador jugador;
    //private int numeroPokemonSeleccionado;

     void start()
    {
    //await crearJugadorPrueba(); //Pureba para poder probar la opcio ver equipo pokemon
    //jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Jugador;//Esta a null porque en playercontroller a la par se esta creando alli,
    //Si lo quiero probar hacerlo en una corrutina. esperar unos segundos y luego obtener el jugador
        //textNumeroPokemons.text = $"Equipo Actual {jugador.EquipoPokemon.Count}/6";
        //Utilidades.prepararBotonesPokemonsEquipo(jugador.EquipoPokemon, botonesPokemons);
    }

    public void verOpcionesPokemon() //Asignar este metodo a los botones en el inspector
    {

        /* EventSystem.current.currentSelectedGameObject.name me dara el nombre del GameObject seleccionado, En este caso el de un boton,
         * como los botones su nombre es ButtonPokemon 1, ButtonPokemon 2, ect. Se coge el ultimo caracter del nombre que 
         * corresponde al numero del boton clicado 
         */
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name;
        int numeroBotonPulsado = nombreBoton[nombreBoton.Length - 1];
        PokemonJugador pokemon = jugador.EquipoPokemon[numeroBotonPulsado - 1];

        menuOpcionesPokemon.GetComponent<MenuOpcionesPokemon>().Pokemon = pokemon;//scriptPlayer.Jugador.EquipoPokemon[numeroPokemon
        menuOpcionesPokemon.GetComponent<MenuOpcionesPokemon>().cambiarTextoTextNombrePokemon(); //Mejor hacerlo aqui porque es mas optimo, la otra forma seria en la clase MenuOpcionesPokemon hacer el cambio de nombre en un metodo update y eso no es lo mas optimo
        menuOpcionesPokemon.SetActive(true);

    }
    /*
     * Se hace asi porque delegate { verOpcionesPokemon(pokemon.PokemonNumero); }
     * 
     * De esta manera no se le puede incluir parametros por eso poner lo de delegate
     * botonesPokemons[0].onClick.AddListener(verOpcionesPokemon(pokemon.PokemonNumero))
     */

}

