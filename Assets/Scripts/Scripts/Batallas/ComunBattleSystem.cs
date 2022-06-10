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
    public GameObject textSinItems;
    //Propiedades publicas, para que las clases que hereden de estas puedan usarlas
    public BattleState BattleState { get; set; }
    public Jugador Jugador { get; set; }
    public Pokemon PokemonRivalLuchando { get; set; }
    public PokemonJugador PokemonJugadorLuchando { get; set; }
    public ItemConCantidad ItemAUsar { get; set; }


    public GameObject InterfazItemAUsar { get; set; }

    public readonly int PROBABILIDAD_CRITICO = 2;

    private PokemonJugador pokemonSeleccionado;

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
        GameObject plantillaItem = menuMochila.transform.GetChild(2).gameObject,
        contentScroView = menuMochila.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        GameObject interfazItem;
        if (Jugador.Mochila.Count > 0)
        {
            textSinItems.SetActive(false);
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
        else {
            textSinItems.SetActive(true);
        }
    }
    /// <summary>
    /// Cabecera: public void prepararIconosPokemosDisponibles(List<Pokemon> equipoPokemon, GameObject interfazPokemonsDisponibles)
    /// Comentario: Este metodo se encarga preparar y configurar la interfaz que muestra los pokemons disponibles que tiene un jugador.
    /// Entradas: List<Pokemon> equipoPokemon, GameObject interfazPokemonsDisponibles
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Interfaz de pokemons disponibles del jugador configurada. Si se produce alguna excepcion, no se configurara la interfaz.   
    /// <parama name="equipoPokemon"></param>
    /// <parama name="interfazPokemonsDisponibles"></param>
    /// </summary>
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

    /// <summary>
    /// Cabecera: public void prepararIconosPokemosDisponibles(List<Pokemon> equipoPokemon, GameObject interfazPokemonsDisponibles)
    /// Comentario: Este metodo se encarga de preparar y configurar las interfaces de los movimientos que se encuentran en la zona de atacar de un pokemon.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Interfaces de movimientos de ataque de un pokemon configuradas.    
    /// </summary>
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

    /// <summary>
    /// Cabecera: public void abandonarBatallaButton()
    /// Comentario: Este metodo se encarga de realizar las operaciones basicas para cuando sea el turno del jugador.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se indicara que es el turno del jugador y se activara la iteracion de los botones principales del combate. 
    /// </summary>
    public void turnoJugador()
    {
        BattleState = BattleState.PLAYERTURN;
        textoDialogo.text = $"¿Que hara {PokemonJugadorLuchando.Nombre}?";
        activarDesactivarBotonesMenuAcciones(true);
    }
    
    /// <summary>
    /// Cabecera: public bool determinarDerrotaJugador()
    /// Comentario: Este metodo se encarga de comprobarar si el jugador tiene algun pokemon vivo.
    /// Entradas: Ninguna
    /// Salidas: bool
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un booleano que puede tomar dos valores:
    ///                  true: Si el jugador no tiene ningun pokemon con vida.
    ///                  false:Si el jugador tiene algun pokemon con vida.
    /// <returns>bool<returns>
    /// </summary>
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

    /// <summary>
    /// Cabecera: public void activarDesactivarMenuEquipo(bool activarBoton, bool activarMenu)
    /// Comentario: Este metodo se encarga de activar/Desactivar el menu del equipo del jugador y bloquea/Activar el boton de salir del menu.
    /// Entradas: bool activarBoton, bool activarMenu
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se activara o desactivara el menu equipo de combate del jugador y su boton de salir. 
    /// <param name="activarBoton"></param>
    /// <param name="activarMenu"></param>
    /// </summary>
    public void activarDesactivarMenuEquipo(bool activarBoton, bool activarMenu)
    {
        var botonAtras = menuEquipo.transform.Find("ButtonAtrasMenuEquipo");
        botonAtras.GetComponent<Button>().interactable = activarBoton;
        menuEquipo.SetActive(activarMenu);
    }

    /// <summary>
    /// Cabecera: public void activarDesactivarBotonesMenuAcciones(bool estado)
    /// Comentario: Este metodo se encarga de activar/Desactivar los botones del menu de acciones del combate.
    /// Entradas: bool estado
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se activara/Desactivara los botones del menu de acciones del combate.  
    /// <param name="estado"></param>
    /// </summary>
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
            //Boton del menu equipo que corresponde a ver los movimientos de un pokemon en especifico
            botonesPokemonsEquipo[i].gameObject.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = !activacion;
            //Boton que corresponde que corresponde a ver los datos de un pokemon en especifico
            botonesPokemonsEquipo[i].gameObject.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = !activacion;
        }
        menuEquipo.SetActive(activacion);
    }
    /// <summary>
    /// Cabecera: public void opcionVerDatos(GameObject menuDatos)
    /// Comentario: Este metodo se encarga de recoger los datos necesarios para poder llamar al metodo configurarMenuDatosPokemon de la clase UtilidadesEscena.
    /// Entradas: GameObject menuDatos
    /// Salidas: Ninguna
    /// Precondiciones: menuDatos no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: El menu de datos esta configurado con los valores del pokemon seleccionado
    /// <param name="menuDatos"></param>
    /// </summary>
    public void opcionVerMenuDatos(GameObject menuDatos)
    {
        int PokemonNumero = Int16.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        pokemonSeleccionado = Jugador.EquipoPokemon.Find(g => g.PokemonNumero == PokemonNumero);
        UtilidadesEscena.configurarMenuDatosPokemon(menuDatos, pokemonSeleccionado);
    }
    /// <summary>
    /// Cabecera: public void opcionVerMovimientos(GameObject menu)
    /// Comentario: Este metodo se encarga de recoger los datos necesarios para poder llamar al metodo configurarMostrarMenuMovimientos de la clase UtilidadesEscena.
    /// Entradas: GameObject menu
    /// Salidas: Ninguna
    /// Precondiciones: menu no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: El menu de movimientos esta configurado con los valores del pokemon seleccionado
    /// <param name="menu"></param>
    /// </summary>
    public void opcionVerMovimientos(GameObject menu)
    {
        UtilidadesEscena.configurarMostrarMenuMovimientos(menu, pokemonSeleccionado);
    }
    
    /// <summary>
    /// Cabecera: public void configurarDerrotaJugador()
    /// Comentario: Este metodo se encarga de realizar las operaciones necesarias para cuando el jugador sea derrotado 
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se saldra de la escena de combate y se llevara al jugador a la escena PokemonCenter 
    /// </summary>
    public void configurarDerrotaJugador()
    {
        PlayerPrefs.SetString("NameLastScene", SceneManager.GetSceneAt(1).name);
        StopAllCoroutines();
        foreach (Pokemon pokemon in Jugador.EquipoPokemon) //Se cura todo el equipo pokemon del jugador
        {
            pokemon.HP = pokemon.HPMaximos;
        }
        UtilidadesEscena.eliminarGameObjectsItemsYEntrenadores();
        Jugador.Dinero -= UnityEngine.Random.Range(100, 301);//Dinero que se pierde por perder la batalla
        UtilidadesEscena.precargarEscena("PokemonCenterScene"); //Se lleva al jugador a el centro pokemon
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
    }
    
    /// <summary> 
    /// Anotacion: Este metodo puede causar que el programa se quede congelado, ya que se realiza una llamada a una API y esta puede no responder, congelando asi el programa 
    ///
    /// Cabecera: public async Task<Pokemon> generarObtenerPokemonRival()
    /// Comentario: Este metodo se encarga de realizar las operaciones necesarias para obtener un pokemon rival
    /// Entradas: Ninguna
    /// Salidas: Task<Pokemon> pokemonGenerado
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un pokemon configurado.
    /// <returns name="pokemonGenerado"></returns>
    /// </summary>
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
    //Metodo que añade a un pokemon un movimiento por defecto hasta que este tenga 4 movimiento. Si ya tiene 4 movimiento no se añade ninguno.
    private void anhadirMovimientosFaltantes(Pokemon pokemonGenerado)//Metodo por si el pokemon que vienen de la pokeApi no tiene 4 movimientos
    {
        for (int i = pokemonGenerado.Movimientos.Count; i < 4; i++)
        {
            Debug.Log("Pokemon sin 4 movimientos");
            pokemonGenerado.Movimientos.Add(new MovimientoPokemon(999,"Daño secreto",50,100,15,"Normal"));
        }
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
