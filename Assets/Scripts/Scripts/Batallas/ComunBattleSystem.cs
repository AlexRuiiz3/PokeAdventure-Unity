using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum BattleState { START, WIN, LOST, PLAYERTURN, ENEMYTURN, POKEMONJUGADORDEBILITADO };
public class ComunBattleSystem : MonoBehaviour
{
    //Atributos publicos para que desde unity se le pueda asignar un valor
    public GameObject imagenBackGround;
    public TextMeshProUGUI textoDialogo;
    public TrainerHUD trainerHUD;
    public RivalPokemonHUD rivalPokemonHUD;
    public List<Button> botonesPokemonsEquipo;
    public List<Button> botonesMovimientos;
    public GameObject menuEquipo;
    public GameObject menuAtaque;
    public GameObject menuMochila;
    public GameObject pantallaCarga;
    public AudioSource audio;
    //Propiedades publicas, para que las clases que hereden de estas puedan usarlas
    public BattleState BattleState { get; set; }
    public Jugador Jugador { get; set; }
    public Pokemon PokemonRivalLuchando { get; set; }
    public PokemonJugador PokemonJugadorLuchando { get; set; }
    public ItemConCantidad ItemAUsar { get; set; }

    public GameObject InterfazItemAUsar { get; set; }

    public readonly int PROBABILIDAD_CRITICO = 2;


    public void prepararConfigurarDatosJugador()
    {
        PokemonJugadorLuchando = (from pokemon in Jugador.EquipoPokemon
                                  where pokemon.HP > 0
                                  select pokemon).First();
        trainerHUD.inicializarDatos(PokemonJugadorLuchando);
        prepararIconosPokemosDisponibles(Jugador.EquipoPokemon.Cast<Pokemon>().ToList(), trainerHUD.pokemonsDisponibles);
        UtilidadesEscena.prepararBotonesPokemonsEquipo(Jugador.EquipoPokemon, botonesPokemonsEquipo);
        prepararBannerIconosMovimientos();
    }
    /// <summary>
    /// Cabecera: public void configurarMenuMochila()
    /// Comentario: Este metodo se encarga de configurar y preparar el apartado del la interfaz del menu de la mochila del jugador.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se vizualiza el menu de la mochila del jugador con los items que contiene.         
    /// </summary>
    public void configurarMenuMochila()
    {
        GameObject plantillaItem = menuMochila.transform.GetChild(1).gameObject,
        contentScroView = menuMochila.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        GameObject interfazItem;

        foreach (ItemConCantidad item in Jugador.Mochila)
        {

            interfazItem = Instantiate(plantillaItem);
            //Imagen
            interfazItem.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Imagenes/Items/{item.Nombre}");
            //Text Nombre
            interfazItem.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = (item.CuracionPS != 0) ? $"{item.Nombre}. {item.CuracionPS}PS" : item.Nombre;
            //Text Cantidad
            interfazItem.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{item.Cantidad}";
            if (SceneManager.GetSceneByName("BattleTrainerScene").isLoaded && item.Tipo == "Pokeball")
            {
                Destroy(interfazItem.transform.Find("Button").gameObject);
            }
            //Asignacion al content del scrollView
            interfazItem.transform.SetParent(contentScroView.transform);
            interfazItem.SetActive(true);
        }
    }

    public void prepararIconosPokemosDisponibles(List<Pokemon> equipoPokemon, GameObject interfazPokemonsDisponibles)
    {
        UtilidadesEscena.eliminarHijosGameObject(interfazPokemonsDisponibles);
        GameObject gameObjectImagen;
        Image imagen;
        //RectTransfor almacena la posicion, tamaño, anclaje y pivote de una rectangulo. En este caso el gameObject pokemonsDisponibles es un rectangulo(Donde se almacenaran las imagenes)
        RectTransform rt = interfazPokemonsDisponibles.GetComponent<RectTransform>();
        //Un gameObject solo puede tener un objeto grafico, por eso en el for se tiene que crear un nuevo gameObject y asignarle la imagen
        for (int i = 0; i < equipoPokemon.Count; i++)
        {
            gameObjectImagen = new GameObject();

            imagen = gameObjectImagen.AddComponent<Image>(); //Ua nueva imagen que se añadira automaticamente al gameObject(gameObjectImagen)
            if (equipoPokemon[i].HP > 0)
            {
                imagen.sprite = Resources.Load<Sprite>("Imagenes/UI/EscenasBatalla/icon_ball");
            }
            else
            {
                imagen.sprite = Resources.Load<Sprite>("Imagenes/UI/EscenasBatalla/icon_ball_faint");
            }

            //De esta forma se mofidica el ancho y alto de la imagen 
            imagen.rectTransform.sizeDelta = new Vector2(20f, 20f);

            //Al nuevo gameObject con la imagen su transform(x,y,z) padre se modifica para que sea el del gameObject pokemonsDisponibles, asi la imagen se incluira en el gameObject pokemonsDisponibles
            //Al indicarle false es como decirle que se ponga dentro/junto de rt(rt es el recuadro del gameObject pokemonDisponibles) que pasara a ser su padre ahora, True indicara que tome la posicion global de la escena.
            gameObjectImagen.transform.SetParent(rt, false);
        }
    }

    //Metodo que configura la interfaz de los movimientos del pokemon del jugador que este luchando con los datos de sus movimientos
    public void prepararBannerIconosMovimientos()
    {
        Image imagenMovimiento, imagenNombreTipo;
        TextMeshProUGUI textNombre, textPotencia, textPP, textPrecicion;
        MovimientoPokemon movimiento;
        List<Component> componentesBoton = new List<Component>();
        for (int i = 0; i < PokemonJugadorLuchando.Movimientos.Count; i++)
        {
            movimiento = PokemonJugadorLuchando.Movimientos[i];
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

    /// <summary>
    /// Cabecera: public void abandonarBatallaButton()
    /// Comentario: Este metodo se encarga salir de una batalla.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se abandona la escena de batalla, se vuelve a la escena anterior y se vuelve activar el gameObject del jugador 
    /// </summary>
    public void abandonarBatallaButton()
    {
        StopAllCoroutines();
        PlayerPrefs.SetString("NameLastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).name);
        UtilidadesEscena.activarPausarMusicaEscenaActiva(true);
        GameObject player = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        player.SetActive(true);
    }

    //Configuracion basica para cuando sea el turno de jugador
    public void turnoJugador()
    {
        BattleState = BattleState.PLAYERTURN;
        textoDialogo.text = $"¿Que hara {PokemonJugadorLuchando.Nombre}?";
        activarDesactivarBotonesMenuAcciones(true);
    }
    //Metodo que comprobara si el jugador tiene algun pokemon vivo para seguir luchando
    public bool determinarDerrotaJugador()
    {
        bool derrota = true;
        if (UtilidadesSystemaBatalla.comprobarPokemonsVivos(Jugador.EquipoPokemon.Cast<Pokemon>().ToList()))
        {
            activarDesactivarMenuEquipo(false, true);
            derrota = false;
            BattleState = BattleState.POKEMONJUGADORDEBILITADO;
        }
        return derrota;
    }

    //Metodo que activa el menu del equipo del jugador y bloquea el boton de salir del menu
    public void activarDesactivarMenuEquipo(bool activarBoton, bool activarMenu)
    {
        var botonAtras = menuEquipo.transform.Find("ButtonAtrasMenuEquipo");
        botonAtras.GetComponent<Button>().interactable = activarBoton;
        menuEquipo.SetActive(activarMenu);
        //c.interactable = activarBoton;
    }

    //Metodo que bloquea los botones de la acciones principales que puede elegir un jugador en la batalla
    public void activarDesactivarBotonesMenuAcciones(bool estado)
    {
        GameObject a = GameObject.Find("ActionsZone");
        foreach (Button boton in a.GetComponentsInChildren<Button>())
        {
            boton.interactable = estado;
        }
    }

    /// <summary>
    /// Cabecera: public void configurarMenuEquipo(bool activacion)
    /// Comentario: Este metodo se encarga de configurar la activacion de todos los botones que hay en el menu equipo del jugador.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran dos acciones principales en funcion del booleano recibido como parametro:
    ///                  1:Si activacion es true, menu equipo se activara, los botones correspondientes a un pokemon seran interactables y los botones Movimientos y Datos que estan asociado a cada pokemon, no seran interactables.
    ///                  2:Si activacion es false, menu equipo se desactivara, los botones correspondientes a un pokemon dejaran de ser interactables y los botones Movimientos y Datos que estan asociado a cada pokemon, volveran a ser interactables.
    /// </summary>
    /// <param name="activacion"></param>
    public void configurarMenuEquipo(bool activacion)
    {
        for (int i = 0; i < Jugador.EquipoPokemon.Count; i++)
        {
            if (activacion && Jugador.EquipoPokemon[i].HP > 0 && Jugador.EquipoPokemon[i].HP < Jugador.EquipoPokemon[i].HPMaximos)
            {
                botonesPokemonsEquipo[i].interactable = true;
            }
            else
            {
                botonesPokemonsEquipo[i].interactable = false;
            }
            //Boton que corresponde a un pokemon

            //Boton del menu equipo que corresponde a ver los movimientos de un pokemon en especifico
            botonesPokemonsEquipo[i].gameObject.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = !activacion;
            //Boton que corresponde que corresponde a ver los datos de un pokemon en especifico
            botonesPokemonsEquipo[i].gameObject.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = !activacion;
        }
        menuEquipo.SetActive(activacion);
    }

    public async Task<Pokemon> generarObtenerPokemonRival()
    {

        int nivelWildPokemon = UtilidadesSystemaBatalla.determinarNivelPokemonRival(Jugador.EquipoPokemon);
        PokeAPI.Pokemon wildPokemonApi = await APIListadosPokemonBL.obtenerPokemonDeApi(UnityEngine.Random.Range(1, 899)); //;
        Pokemon pokemonGenerado = new Pokemon(wildPokemonApi);
        pokemonGenerado.Nivel = nivelWildPokemon;
        pokemonGenerado.establecerEstadisticasAlNivelActual();
        await pokemonGenerado.obtenerDatosAsincronos(wildPokemonApi);
        anhadirMovimientosFaltantes(pokemonGenerado);
        return pokemonGenerado;
    }
    private void anhadirMovimientosFaltantes(Pokemon pokemonGenerado)//Metodo por si el pokemon que vienen de la pokeApi no tiene 4 movimientos
    {
        for (int i = pokemonGenerado.Movimientos.Count; i < 4;)
        {
            pokemonGenerado.Movimientos.Add(new MovimientoPokemon(999,"Daño secreto",50,100,15,"Normal"));
        }
    }
    public void configurarDerrotaJugador()
    {
        PlayerPrefs.SetString("NameLastScene", SceneManager.GetSceneAt(1).name);
        StopAllCoroutines();
        foreach (Pokemon pokemon in Jugador.EquipoPokemon)
        {
            pokemon.HP = pokemon.HPMaximos;
        }
        UtilidadesEscena.eliminarGameObjectsItemsYEntrenadores();
        Jugador.Dinero -= UnityEngine.Random.Range(100, 301);//Dinero que se pierde por perder la batalla
        UtilidadesEscena.precargarEscena("PokemonCenterScene");
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
    }
    /*
    //Metodo que se encarga generar y configurar un pokemon rival de forma aleatoria
    public async Task<Pokemon> generarObtenerPokemonRival()
    {
        Pokemon pokemonGenerado = null;
        bool datosAsincronosObtenidos = false;

        while (pokemonGenerado is null || !datosAsincronosObtenidos) {
            try
            {
                int random = UnityEngine.Random.Range(898, 901);
                Debug.Log("Obteniendo pokemon de PokeApi: random:"+random);
                CancellationTokenSource s_cts = new CancellationTokenSource();
                s_cts.CancelAfter(5000);
                int nivelWildPokemon = UtilidadesSystemaBatalla.determinarNivelPokemonRival(Jugador.EquipoPokemon);
                Task<PokeAPI.Pokemon> b = APIListadosPokemonBL.obtenerPokemonDeApi(random);
                b.Wait(s_cts.Token);
                PokeAPI.Pokemon wildPokemonApi = await b;//UnityEngine.Random.Range(1, 899));
                pokemonGenerado = new Pokemon(wildPokemonApi);
                pokemonGenerado.Nivel = nivelWildPokemon;
                pokemonGenerado.establecerEstadisticasAlNivelActual();
                Task<bool> a = pokemonGenerado.obtenerDatosAsincronos(wildPokemonApi);
                a.Wait(s_cts.Token);
                datosAsincronosObtenidos = await a;
            }
            catch (Exception) {
                Debug.Log("Ocurrio un error PokeApi");
                pokemonGenerado = null;
                datosAsincronosObtenidos = false;
            }
        }
        return pokemonGenerado;
    }*/
}
