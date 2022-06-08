using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObtenerPrimerPokemon : MonoBehaviour
{
    public List<GameObject> menusPokemons;
    public GameObject contenerdorBotonesPokeballs;
    private bool botonesActivados;
    private string identificadorPokemonSeleccionado;
    private List<Pokemon> pokemonsIniciales;

    void Start()
    {
        pokemonsIniciales = new List<Pokemon>();
        FindObjectOfType<ControlDialogos>().activarDialogo();
        generarPokemonsIniciales();
        configurarMenusPokemons();
    }

    private void Update()
    {
        if (!botonesActivados && PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString())
        {
            contenerdorBotonesPokeballs.SetActive(true);
            botonesActivados = true;
        }
    }

    private void configurarMenusPokemons() {
        Pokemon pokemon;
        for (int i = 0; i < menusPokemons.Count; i++) {
            pokemon = pokemonsIniciales[i];
            menusPokemons[i].GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
            menusPokemons[i].GetComponentsInChildren<Image>()[2].sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/IconosNombre/" + pokemon.Tipos[0]).First();
            if (pokemon.Tipos.Count > 1) {
                menusPokemons[i].GetComponentsInChildren<Image>()[2].sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/IconosNombres/" + pokemon.Tipos[0]).First();
            }
            menusPokemons[i].GetComponentsInChildren<Text>()[0].text = pokemon.Nombre;
            menusPokemons[i].GetComponentsInChildren<Text>()[1].text = pokemon.HP.ToString();
            menusPokemons[i].GetComponentsInChildren<Text>()[2].text = pokemon.Ataque.ToString();
            menusPokemons[i].GetComponentsInChildren<Text>()[3].text = pokemon.Defensa.ToString();
            menusPokemons[i].GetComponentsInChildren<Text>()[4].text = pokemon.Velocidad.ToString();
        }
    }

    public void mostrarMenuPokemonSeleccionado(GameObject menuPokemon) {
        identificadorPokemonSeleccionado = menuPokemon.name;
        menuPokemon.SetActive(true);
    }

    public void buttonAceptarSeleccionarPokemon()
    {
        PokemonJugador pokemonSeleccionado = determinarPokemonSeleccionado();
        ClsJugador clsJugador;
        try
        {
            //Se obtiene el jugador, para que venga ya con un id
            clsJugador = ListadosJugadorBL.obtenerJugador(PlayerPrefs.GetString("NombreUsuarioIniciado"),PlayerPrefs.GetString("ContrasenhaUsuarioIniciado"));
            GestoraPokemonEncontradosJugadorBL.insertarPokemonsEncontradosAJugador(clsJugador.ID, new List<PokemonEncontrado> { new PokemonEncontrado(pokemonSeleccionado.ID, pokemonSeleccionado.Nombre) });
            pokemonSeleccionado.IDJugador = clsJugador.ID;
            GestoraPokemonsJugadorBL.guardarPokemonsDeJugador(clsJugador.ID, new List<PokemonJugador> { pokemonSeleccionado});

            Utilidades.obtenerDatosJugador(clsJugador.NombreUsuario,clsJugador.Contrasenha);
            UtilidadesEscena.precargarEscena("LobbyScene");
        }
        catch (Exception) { 
            Debug.Log("Error trabajando con los datos del jugador");
        }
    }

    private PokemonJugador determinarPokemonSeleccionado()
    {
        PokemonJugador pokemon = new PokemonJugador();
        switch (identificadorPokemonSeleccionado)
        {

            case "MenuPokemonFuego":
                pokemon = new PokemonJugador(pokemonsIniciales[0],0,1,1,0,100);
                break;

            case "MenuPokemonAgua":
                pokemon = new PokemonJugador(pokemonsIniciales[1], 0, 1, 1, 0, 100);
                break;

            case "MenuPokemonPlanta":
                pokemon = new PokemonJugador(pokemonsIniciales[2], 0, 1, 1, 0, 100);
                break;

        }
        return pokemon;
    }

    private void generarPokemonsIniciales()
    {
        List<MovimientoPokemon> movimientosPokemonsFuego = new List<MovimientoPokemon> { new MovimientoPokemon(1, "Ascuas", 40, 100, 25, "Fuego"), new MovimientoPokemon(2, "Lanzallamas", 90, 100, 15, "Fuego"), new MovimientoPokemon(3, "Sorpresa", 40, 100, 10, "Normal"), new MovimientoPokemon(4, "Canon", 60, 100, 15, "Normal") },
            movimientosPokemonsAgua = new List<MovimientoPokemon> { new MovimientoPokemon(5, "Doble golpe", 35, 90, 10, "Normal"), new MovimientoPokemon(6, "Rayo burbuja", 65, 100, 20, "Agua"), new MovimientoPokemon(7, "Surf", 90, 100, 25, "Agua"), new MovimientoPokemon(8, "Arañazo", 40, 100, 35, "Normal") },
            movimientosPokemonsPlanta = new List<MovimientoPokemon> { new MovimientoPokemon(9, "Corte", 50, 95, 30, "Normal"), new MovimientoPokemon(10, "Hoja Magica", 60, 100, 20, "Planta"), new MovimientoPokemon(11, "Destructor", 40, 100, 35, "Normal"), new MovimientoPokemon(12, "Rayo solar", 120, 100, 10, "Planta") };
        List<string> debilidadesPlanta = new List<string> { "Fuego", "Bicho", "Hielo", "Volador", "Veneno" },
            debilidadesFuego = new List<string> { "Agua", "Tierra", "Roca" },
            debilidadesAgua = new List<string> { "Planta", "Electrico" };
        List<string> tipoPlanta = new List<string> { "Planta" },
             tipoAgua = new List<string> { "Agua" },
              tipoFuego = new List<string> { "Fuego" };

        List<Pokemon> pokemonsInicialesPlanta = new List<Pokemon>{ new Pokemon(152,"Chikorita",45,45,1,49,65,45,movimientosPokemonsPlanta,tipoPlanta,debilidadesPlanta),
            new Pokemon(252, "Treecko", 40, 40, 1, 45, 35, 70, movimientosPokemonsPlanta, tipoPlanta, debilidadesPlanta),
            new Pokemon(387,"Turtwig",55,55,1,68,68,31,movimientosPokemonsPlanta,tipoPlanta,debilidadesPlanta),
            new Pokemon(495,"Snivy",45,45,1,45,55,63,movimientosPokemonsPlanta,tipoPlanta,debilidadesPlanta),
            new Pokemon(650,"Chespin",56,56,1,61,65,38,movimientosPokemonsPlanta,tipoPlanta,debilidadesPlanta),
            new Pokemon(722,"Rowlet",68,68,1,55,55,42,movimientosPokemonsPlanta,new List<string>{ "Planta","Volador"},debilidadesPlanta),
            new Pokemon(810,"Grookey",50,50,1,65,50,65,movimientosPokemonsPlanta,tipoPlanta,debilidadesPlanta)},
        pokemonsInicialesAgua = new List<Pokemon>{ new Pokemon(158,"Totodile",50,50,1,65,64,43,movimientosPokemonsAgua,tipoAgua,debilidadesAgua),
            new Pokemon(258, "Mudkip", 50, 50, 1, 70, 50, 40, movimientosPokemonsAgua, tipoAgua, debilidadesAgua),
            new Pokemon(393,"Piplup",53,53,1,51,53,40,movimientosPokemonsAgua,tipoAgua,debilidadesAgua),
            new Pokemon(501,"Oshawott",55,55,1,55,45,45,movimientosPokemonsAgua,tipoAgua,debilidadesAgua),
            new Pokemon(656,"Froakie",41,41,1,56,71,38,movimientosPokemonsAgua,tipoAgua,debilidadesAgua),
            new Pokemon(728,"Popplio",50,50,1,54,54,40,movimientosPokemonsAgua,tipoAgua,debilidadesAgua),
            new Pokemon(816,"Sobble",50,50,1,40,40,70,movimientosPokemonsAgua,tipoAgua,debilidadesAgua)},
        pokemonsInicialesFuego = new List<Pokemon>{ new Pokemon(155,"Cyndaquil",39,39,1,52,43,65,movimientosPokemonsFuego,tipoFuego,debilidadesFuego),
            new Pokemon(255, "Torchic", 45, 45, 1, 60, 40, 45, movimientosPokemonsFuego, tipoFuego, debilidadesFuego),
            new Pokemon(390,"Chimchar",44,44,1,58,44,61,movimientosPokemonsFuego,tipoFuego,debilidadesFuego),
            new Pokemon(498,"Tepig",65,65,1,63,45,45,movimientosPokemonsFuego,tipoFuego,debilidadesFuego),
            new Pokemon(653,"Fennekin",40,40,1,45,40,60,movimientosPokemonsFuego,tipoFuego,debilidadesFuego),
            new Pokemon(725,"Litten",45,45,1,65,40,70,movimientosPokemonsFuego,tipoFuego,debilidadesFuego),
            new Pokemon(813,"Scorbunny",50,50,1,71,40,69,movimientosPokemonsFuego,tipoFuego,debilidadesFuego)};

        pokemonsIniciales.Add(pokemonsInicialesFuego[UnityEngine.Random.Range(0, pokemonsInicialesPlanta.Count)]);
        pokemonsIniciales.Add(pokemonsInicialesAgua[UnityEngine.Random.Range(0, pokemonsInicialesPlanta.Count)]);
        pokemonsIniciales.Add(pokemonsInicialesPlanta[UnityEngine.Random.Range(0, pokemonsInicialesPlanta.Count)]);
    }
}
