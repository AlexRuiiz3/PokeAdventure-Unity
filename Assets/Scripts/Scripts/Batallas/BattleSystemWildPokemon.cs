using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, WIN, LOST, PLAYERTURN, ENEMYTURN, POKEMONJUGADORDEBILITADO };
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

    private readonly int PROBABILIDAD_CRITICO = 2;
    async void Start()
    {
        GameObject.Find("MenuEquipo").SetActive(false);//Al crearlos desde unity, estan por defecto visible
        GameObject.Find("MenuAtaque").SetActive(false);
        GameObject.Find("MenuAtaque").SetActive(false);
        //se busca al jugador desde resource, ya que se encuentra desabilitado
        jugador = Resources.FindObjectsOfTypeAll<GameObject>()
                           .FirstOrDefault(g => g.CompareTag("Player"))
                           .GetComponent<PlayerController>().Jugador;


        await prepararPokemonRival();
        StartCoroutine(prepararBatalla());
    }

    IEnumerator prepararBatalla()//Se hace en una corrutina para poder poner pausa y que los mensajes que se muestran no se cambien tan rapido
    {
        textoDialogo.text = $"Un {wildPokemon.Nombre} salvaje aparecio!";
        pokemonJugadorLuchando = (from pokemon in jugador.EquipoPokemon
                                  where pokemon.HP > 0
                                  select pokemon).First();
        trainerHUD.inicializarDatos(pokemonJugadorLuchando.Nombre, pokemonJugadorLuchando.Nivel, pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos, pokemonJugadorLuchando.ImagenDeEspalda);
        trainerHUD.prepararIconosPokemosDisponibles(jugador.EquipoPokemon.Count);
        wildPokemonHUD.inicializarDatos(wildPokemon);
        Utilidades.prepararBotonesPokemonsEquipo(jugador.EquipoPokemon, botonesPokemonsEquipo);
        prepararBannerIconosMovimientos();

        int aleatorioComienzo = UnityEngine.Random.Range(1, 3); //Aleatorio en entre 1 y 2

        if (aleatorioComienzo == 1)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
        }
        else
        {
            activarDesactivarBotonesMenuAcciones(false);
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
        }
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
            textPrecicion.text = $"Precicion: {movimiento.Precicion}";

            componentesBoton.Clear(); //Se limpia la lista con los componentes del boton para que despues guardar los componentes del siguiente boton y asi las posicion 1,2,3,4 corresponderan a los componentes del boton que le toque en la iteracion
        }
    }

    public void cambiarPokemon()
    {
        if (battleState == BattleState.PLAYERTURN || battleState == BattleState.POKEMONJUGADORDEBILITADO)
        {
            string nombreBoton = EventSystem.current.currentSelectedGameObject.transform.parent.name;
            int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);
            //Controlar esto jugador.EquipoPokemon[numeroBotonPulsado - 1].PokemonNumero != pokemonJugadorLuchando.PokemonNumero;
            //Para que cuando se quiera cambiar por el pokemon que ya esta luchando no se haga el cambio.
            if (pokemonJugadorLuchando.NumeroEquipado != jugador.EquipoPokemon[numeroBotonPulsado - 1].NumeroEquipado)
            {
                if (jugador.EquipoPokemon[numeroBotonPulsado - 1].HP > 0) //Si la vida del pokemon al que quiere cambiar en mayor que 0
                {
                    configuracionPokemonJugadorDebilitado(true, false);
                    pokemonJugadorLuchando = jugador.EquipoPokemon[numeroBotonPulsado - 1];
                    trainerHUD.inicializarDatos(pokemonJugadorLuchando.Nombre, pokemonJugadorLuchando.Nivel, pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos, (pokemonJugadorLuchando.ImagenDeEspalda) != null ? pokemonJugadorLuchando.ImagenDeEspalda : pokemonJugadorLuchando.ImagenDeFrente);
                    prepararBannerIconosMovimientos();
                    textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} te elijo a ti";
                    if (battleState == BattleState.PLAYERTURN)
                    {
                        activarDesactivarBotonesMenuAcciones(false);
                        battleState = BattleState.ENEMYTURN;
                        StartCoroutine(atacarWildPokemon());
                    }
                    else//(battleState == BattleState.POKEMONJUGADORDEBILITADO)
                    {
                        turnoJugador();
                    }
                }
                else
                {
                    textoDialogo.text = $"{jugador.EquipoPokemon[numeroBotonPulsado - 1].Nombre} no tiene fuerzas para luchar";
                }

            }
            else
            {
                textoDialogo.text = $"{jugador.EquipoPokemon[numeroBotonPulsado - 1].Nombre} ya esta luchando";
            }

        }
        else
        {
            Debug.Log("No es tu turno");
        }
    }

    public void abandonarBatallaButton()
    {
        StopAllCoroutines();
        PlayerPrefs.SetString("NameLastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).name);
        GameObject player = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        player.SetActive(true);
    }

    private void turnoJugador()
    {
        battleState = BattleState.PLAYERTURN;
        textoDialogo.text = $"¿Que hara {pokemonJugadorLuchando.Nombre}?";
        activarDesactivarBotonesMenuAcciones(true);
    }
    public void usarMovimientoJugadorButton(int numeroBotonMovimiento)
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            StartCoroutine(atacarJugador(numeroBotonMovimiento - 1));
        }
    }

    IEnumerator atacarJugador(int numeroBotonPulsado)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            activarDesactivarBotonesMenuAcciones(false);
            GameObject.Find("MenuAtaque").SetActive(false);
            yield return new WaitForSeconds(2f); //Para que no se junte con los mensaje del enemigo, se hace una pausa y asi da tiempo de ver los mensajes de ambos
            MovimientoPokemon movimientoUsado = pokemonJugadorLuchando.Movimientos[numeroBotonPulsado];

            int aleatorioPrecicion = UnityEngine.Random.Range(1, 100);//num aleatorio entre (1 y 100) 100 es el valor maximo que puede tener la precicion de un movimiento
            int danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad, experienciaGanada;
            bool wildPokemonVivo;
            if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, pues falla
            {
                danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                    PROBABILIDAD_CRITICO, pokemonJugadorLuchando.Nombre, movimientoUsado.Nombre, textoDialogo);
                yield return new WaitForSeconds(2f);
                multiplicadorEfectividad = UtilidadesSystemaBatalla.obtenerMultiplicadorPorEfectividad(
                    wildPokemon.Debilidades, movimientoUsado.Tipo, textoDialogo);

                danhoPokemonCausado = UtilidadesSystemaBatalla.calcularDanhoCausado(pokemonJugadorLuchando.Nivel,
                    danhoMovimiento, multiplicadorEfectividad, pokemonJugadorLuchando.Ataque, wildPokemon.Defensa);

                wildPokemonVivo = wildPokemon.recibirDanho(danhoPokemonCausado);
                wildPokemonHUD.setBarraSalud(wildPokemon.HP, wildPokemon.HPMaximos);
                if (!wildPokemonVivo) //Si el pokemon despues de recibir daño esta vivo
                {
                    experienciaGanada = UtilidadesSystemaBatalla.generarExperienciaDerrotarPokemonRival(wildPokemon.Nivel);
                    textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} ha ganado {experienciaGanada} de experiencia";
                    while (pokemonJugadorLuchando.comprobarSubirNivel()) //Se vulve a comprobar con un while porque cuando sube de nivel puede ser que tenga la experiencia necesaria para subir otra vez de nivel de manera seguida
                    {
                        trainerHUD.setTextNivel(pokemonJugadorLuchando.Nivel);
                        yield return new WaitForSeconds(4f);
                        textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} ha subido al nivel {pokemonJugadorLuchando.Nivel}";
                    }

                    yield return new WaitForSeconds(3f);
                    textoDialogo.text = "Has ganado";
                    battleState = BattleState.WIN;
                    yield return new WaitForSeconds(4f);
                    abandonarBatallaButton();//Aqui se abandonara la corrutina, la escena
                }
            }
            else
            {
                textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} ha fallado ";
            }
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
        }
    }
    IEnumerator atacarWildPokemon()
    {

        yield return new WaitForSeconds(2f);
        textoDialogo.text = "Es el turno del pokemon salvaje";
        yield return new WaitForSeconds(2f); //Para que no se junte con los mensaje del enemigo, se hace una pausa y asi da tiempo de ver los mensajes de ambos
        int aleatorioMoviminento = UnityEngine.Random.Range(0, 4),
             aleatorioPrecicion = UnityEngine.Random.Range(1, 100);
        MovimientoPokemon movimientoUsado = wildPokemon.Movimientos[aleatorioMoviminento];
        int danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad;
        bool pokemonJugadorVivo;
        if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, pues falla
        {
            danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                PROBABILIDAD_CRITICO, wildPokemon.Nombre, movimientoUsado.Nombre, textoDialogo);
            yield return new WaitForSeconds(2f);
            multiplicadorEfectividad = UtilidadesSystemaBatalla.obtenerMultiplicadorPorEfectividad(
                pokemonJugadorLuchando.Debilidades, movimientoUsado.Tipo, textoDialogo);

            danhoPokemonCausado = UtilidadesSystemaBatalla.calcularDanhoCausado(wildPokemon.Nivel,
                danhoMovimiento, multiplicadorEfectividad, wildPokemon.Ataque, pokemonJugadorLuchando.Defensa);

            pokemonJugadorVivo = pokemonJugadorLuchando.recibirDanho(danhoPokemonCausado);
            trainerHUD.setBarraSalud(pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos);
            if (!pokemonJugadorVivo) //Si el pokemon despues de recibir daño no esta vivo
            {
                yield return new WaitForSeconds(2f);
                textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} se ha debilitado";
                if (determinarDerrotaJugador())
                {
                    yield return new WaitForSeconds(2f);
                    textoDialogo.text = "Has Perdido";
                    battleState = BattleState.LOST;
                    yield return new WaitForSeconds(5f);
                    abandonarBatallaButton();//Aqui se abandonara la corrutina, la escena
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    textoDialogo.text = "Elige un pokemon para luchar";
                }
            }
        }
        else
        {
            textoDialogo.text = $"El {wildPokemon.Nombre} salvaje ha fallado ";
        }
        if (battleState == BattleState.ENEMYTURN)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
        }
    }

    private bool determinarDerrotaJugador()
    {
        bool derrota = true;

        var pokemonsJugadorVivos = (from pokemon in jugador.EquipoPokemon
                                    where pokemon.HP > 0
                                    select pokemon).FirstOrDefault(); //Devolvera null si no hay ninguno.

        if (pokemonsJugadorVivos != null)
        {
            configuracionPokemonJugadorDebilitado(false, true);
            derrota = false;
            battleState = BattleState.POKEMONJUGADORDEBILITADO;//while (pokemonJugadorLuchando.HP == 0) ; //Mientras el jugador no haya cambiado a un pokemon que tenga vida
        }
        return derrota;
    }

    private void configuracionPokemonJugadorDebilitado(bool activarBoton, bool activarMenu)
    {
        var a = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "ButtonAtrasMenuEquipo");
        GameObject b = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "MenuEquipo");
        b.SetActive(activarMenu);
        var c = a.GetComponent<Button>();
        c.interactable = activarBoton;
    }

    private async Task prepararPokemonRival()
    {
        int nivelWildPokemon = UtilidadesSystemaBatalla.determinarNivelPokemonRival(jugador.EquipoPokemon);
        PokeAPI.Pokemon wildPokemonApi = await APIListadosPokemonBL.obtenerPokemonDeApi(UnityEngine.Random.Range(1, 899));
        wildPokemon = new Pokemon(wildPokemonApi);
        wildPokemon.Nivel = nivelWildPokemon;
        await wildPokemon.obtenerDatosAsincronos(wildPokemonApi);
    }

    private void activarDesactivarBotonesMenuAcciones(bool estado)
    {
        GameObject a = GameObject.Find("ActionsZone");
        foreach (Button boton in a.GetComponentsInChildren<Button>())
        {
            boton.interactable = estado;
        }
    }
}
