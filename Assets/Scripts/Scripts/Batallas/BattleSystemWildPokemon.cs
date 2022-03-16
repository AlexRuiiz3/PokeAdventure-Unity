using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, WON, LOST, PLAYERTURN, ENEMYTURN };
public class BattleSystemWildPokemon : MonoBehaviour
{
    private BattleState battleState;
    public TextMeshProUGUI textoDialogo;
    public Jugador jugador;
    public Pokemon wildPokemon;
    public TrainerHUD trainerHUD;
    public WildPokemonHUD wildPokemonHUD;
    public PokemonJugador pokemonJugadorLuchando;
    public List<Button> botonesPokemonsEquipo;
    public List<Button> botonesMovimientos;


    async void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Jugador;
        wildPokemon = new Pokemon(await APIListadosPokemonBL.obtenerPokemonDeApi((int)Random.Range(1, 899))); //Se obtiene un pokemon de forma aleatoria
        textoDialogo.text = $"Un {wildPokemon.Nombre} salvaje aparecio!";
        battleState = BattleState.START;
        prepararBatalla();
        // StartCoroutine(comenzarBatalla());
    }

    private void prepararBatalla()
    {
        pokemonJugadorLuchando = jugador.EquipoPokemon[0];
        trainerHUD.inicializarDatos(pokemonJugadorLuchando.Nombre, pokemonJugadorLuchando.Nivel, pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos, pokemonJugadorLuchando.ImagenDeEspalda);
        trainerHUD.prepararIconosPokemosDisponibles(jugador.EquipoPokemon.Count);
        wildPokemonHUD.inicializarDatos(wildPokemon);
        Utilidades.prepararBotonesPokemonsEquipo(jugador.EquipoPokemon,botonesPokemonsEquipo);
    }
    
    /*
    IEnumerator comenzarBatalla() {


    }*/

}
