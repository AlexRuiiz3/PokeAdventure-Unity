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

     async Task Start()
    {
        //await crearJugadorPrueba(); //Pureba para poder probar la opcio ver equipo pokemon
        //jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Jugador;//Esta a null porque en playercontroller a la par se esta creando alli,
        //Si lo quiero probar hacerlo en una corrutina. esperar unos segundos y luego obtener el jugador
        //textNumeroPokemons.text = $"Equipo Actual {jugador.EquipoPokemon.Count}/6";
        //Utilidades.prepararBotonesPokemonsEquipo(jugador.EquipoPokemon, botonesPokemons);
        gameObject.SetActive(false);

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



    private async Task crearJugadorPrueba()
    {
        try
        {
            ClsJugador b = new ClsJugador(1, "Usuario", "Constrasenha", "Correo", 5, 100, 200, new byte[0]);
            PokeAPI.Pokemon p1 = await APIListadosPokemonBL.obtenerPokemonDeApi(188);
            Pokemon pokemon1 = new Pokemon(p1);
            await pokemon1.obtenerDatosAsincronos(p1);

            PokeAPI.Pokemon p2 = await APIListadosPokemonBL.obtenerPokemonDeApi(651);
            Pokemon pokemon2 = new Pokemon(p2);
            await pokemon2.obtenerDatosAsincronos(p2);

            PokeAPI.Pokemon p3 = await APIListadosPokemonBL.obtenerPokemonDeApi(401);
            Pokemon pokemon3 = new Pokemon(p3);
            await pokemon3.obtenerDatosAsincronos(p3);

            PokeAPI.Pokemon p4 = await APIListadosPokemonBL.obtenerPokemonDeApi(700);
            Pokemon pokemon4 = new Pokemon(p4);
            await pokemon4.obtenerDatosAsincronos(p4);

            PokeAPI.Pokemon p5 = await APIListadosPokemonBL.obtenerPokemonDeApi(289);
            Pokemon pokemon5 = new Pokemon(p5);
            await pokemon5.obtenerDatosAsincronos(p5);

            List<PokemonJugador> equipoPokemon = new List<PokemonJugador>();
            equipoPokemon.Add(new PokemonJugador(pokemon1, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon2, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon3, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon4, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon5, 1, 1, 1, 100));

            List<ItemConCantidad> mochila = new List<ItemConCantidad>();
            mochila.Add(new ItemConCantidad(new Item(1, "Pocion", "Cura 20 hp", 0, 20, "POC"), 10));
            mochila.Add(new ItemConCantidad(new Item(2, "Pokeball", "Dispositivo para capturar pokemons", 0, 20, "POK"), 20));

            jugador = new Jugador(b, equipoPokemon, mochila);
        }
        catch (Exception)
        {
            throw;
        }

    }
}

