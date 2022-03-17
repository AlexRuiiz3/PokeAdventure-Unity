using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
        wildPokemon = new Pokemon(await APIListadosPokemonBL.obtenerPokemonDeApi((int)UnityEngine.Random.Range(1, 899))); //Se obtiene un pokemon de forma aleatoria
        textoDialogo.text = $"Un {wildPokemon.Nombre} salvaje aparecio!";
        battleState = BattleState.START;
        prepararBatalla();
        // StartCoroutine(comenzarBatalla());
    }

    private void prepararBatalla()
    {
        pokemonJugadorLuchando = jugador.EquipoPokemon[1];
        trainerHUD.inicializarDatos(pokemonJugadorLuchando.Nombre, pokemonJugadorLuchando.Nivel, pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos, pokemonJugadorLuchando.ImagenDeEspalda);
        trainerHUD.prepararIconosPokemosDisponibles(jugador.EquipoPokemon.Count);
        wildPokemonHUD.inicializarDatos(wildPokemon);
        Utilidades.prepararBotonesPokemonsEquipo(jugador.EquipoPokemon, botonesPokemonsEquipo);
        prepararBannerIconosMovimientos();
    }

    private void prepararBannerIconosMovimientos()
    {
        Image imagenMovimiento, imagenNombreTipo;
        TextMeshProUGUI textNombre, textPotencia, textPP, textPrecicion;
        MovimientoPokemon movimiento;
        List<Component> componentesBoton = new List<Component>();
        for (int i = 0; i < pokemonJugadorLuchando.Movimientos.Count; i++)
        {
            movimiento = pokemonJugadorLuchando.Movimientos[i];

            foreach (Component componente in botonesMovimientos[i].GetComponentsInChildren<Component>())//Del boton que representa un movimiento, se obtienen los componentes a los que hay que asignarle los datos del movimiento pokemon
            {
                if (componente is TextMeshProUGUI || componente is Image)
                {
                    componentesBoton.Add(componente);
                }
            }
            imagenMovimiento = (Image)componentesBoton[0];
            imagenMovimiento.sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/Banners/" + movimiento.Tipo)[0];
            imagenNombreTipo = (Image)componentesBoton[1];
            imagenNombreTipo.sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/IconosNombre/" + movimiento.Tipo)[0];

            textNombre = (TextMeshProUGUI)componentesBoton[2];
            textNombre.text = movimiento.Nombre;
            textPotencia = (TextMeshProUGUI)componentesBoton[3];
            textPotencia.text = $"Potencia: {movimiento.Danho}";
            textPP = (TextMeshProUGUI)componentesBoton[4];
            textPP.text = $"PP {movimiento.PP}/{movimiento.PPMaximo}";
            textPrecicion = (TextMeshProUGUI)componentesBoton[5];
            textPrecicion.text = $"Precicion: {movimiento.Precision}"; 

            componentesBoton.Clear(); //Se limpia la lista con los componentes del boton para que despues guardar los componentes del siguiente boton y asi las posicion 1,2,3,4 corresponderan a los componentes del boton que le toque en la iteracion
        }
    }

    public void cambiarPokemon() {
        string nombreBoton = EventSystem.current.currentSelectedGameObject.transform.parent.name;
        int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);
        //Contorlar esto jugador.EquipoPokemon[numeroBotonPulsado - 1].PokemonNumero != pokemonJugadorLuchando.PokemonNumero;
        //Para que cuando se quiera cambiar por el pokemon que ya esta luchando no se haga el cambio.
        pokemonJugadorLuchando = jugador.EquipoPokemon[numeroBotonPulsado - 1];
        trainerHUD.inicializarDatos(pokemonJugadorLuchando.Nombre, pokemonJugadorLuchando.Nivel, pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos, pokemonJugadorLuchando.ImagenDeEspalda);
        prepararBannerIconosMovimientos();
    }

    public void huir() {
        //Utilidades.pausarMusicaEscenaActiva();
        //SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name);
        StartCoroutine(waitTwoseconds());

    }

    IEnumerator waitTwoseconds()
    {
        string a = SceneManager.GetSceneAt(0).name;
        string b = SceneManager.GetSceneAt(0).name;
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).name);
        Utilidades.activarMusicaEscenaActiva();
        StopCoroutine(waitTwoseconds());
    }

    /*
    IEnumerator comenzarBatalla() {


    }*/

}
