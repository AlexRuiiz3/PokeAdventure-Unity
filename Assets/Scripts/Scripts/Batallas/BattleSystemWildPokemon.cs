using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum BattleState { START, WIN, LOST, PLAYERTURN, ENEMYTURN, POKEMONJUGADORDEBILITADO };
public class BattleSystemWildPokemon : MonoBehaviour
{
    public GameObject imagenBackGround;
    public TextMeshProUGUI textoDialogo;
    public TrainerHUD trainerHUD;
    public WildPokemonHUD wildPokemonHUD;
    public List<Button> botonesPokemonsEquipo;
    public List<Button> botonesMovimientos;
    public GameObject menuEquipo;
    public GameObject menuAtaque;
    public GameObject menuMochila;

    private BattleState battleState;
    private Jugador jugador;
    private Pokemon wildPokemon;
    private PokemonJugador pokemonJugadorLuchando;
    private ItemConCantidad itemAUsar;
    private GameObject interfazItemAUsar;

    private readonly int PROBABILIDAD_CRITICO = 2;
    
    async void Start()
    {
        //Se obtiene la imagen de fondo del campos de batalla(Se escogera la imagen que corresponde con la escena que esta activa)
        imagenBackGround.GetComponent<Image>().sprite = (from sprite in Resources.LoadAll<Sprite>("Imagenes/UI/EscenasBatalla/BattleBackgrounds")
                                                         where sprite.name == PlayerPrefs.GetString("EscenaAventura")
                                                         select sprite).First();
        //se busca al jugador desde resource, ya que se encuentra desabilitado
        jugador = Resources.FindObjectsOfTypeAll<GameObject>()
                           .FirstOrDefault(g => g.CompareTag("Player"))
                           .GetComponent<PlayerController>().Jugador;


        await prepararPokemonRival();
        configurarMenuMochila();
        StartCoroutine(prepararBatalla());

        PokemonEncontrado pokemonEncontrado = new PokemonEncontrado(wildPokemon.ID,wildPokemon.Nombre);
        if (!DatosGuardarJugador.PokemonsEncontradosJugador.Exists(g => g.Id == pokemonEncontrado.Id)) {
            DatosGuardarJugador.PokemonsEncontradosJugador.Add(pokemonEncontrado);
        }
        //Eso va en el metodo guardar
        //GestoraPokemonEncontradosJugadorBL.insertarPokemonEncontradoAJugador(jugador.ID, wildPokemon.ID, wildPokemon.Nombre);
    }

    /// <summary>
    /// Cabecera:  IEnumerator prepararBatalla()
    /// Comentario: Este corrutina se encarga de configurar y preparar los campos necesarios de una batalla.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Campos de la interfaz configurados en funcion de los datos de los pokemons que se encuentren luchando.
    /// </summary>
    IEnumerator prepararBatalla()//Se hace en una corrutina para poder poner pausa y que los mensajes que se muestran no se cambien tan rapido
    {
        activarDesactivarBotonesMenuAcciones(false);
        textoDialogo.text = $"Un {wildPokemon.Nombre} salvaje aparecio!";
        pokemonJugadorLuchando = (from pokemon in jugador.EquipoPokemon
                                  where pokemon.HP > 0
                                  select pokemon).First();
        trainerHUD.inicializarDatos(pokemonJugadorLuchando);
        trainerHUD.prepararIconosPokemosDisponibles(jugador.EquipoPokemon.Count);
        wildPokemonHUD.inicializarDatos(wildPokemon);
        Utilidades.prepararBotonesPokemonsEquipo(jugador.EquipoPokemon, botonesPokemonsEquipo);
        prepararBannerIconosMovimientos();

        int aleatorioComienzo = 1;//UnityEngine.Random.Range(1, 3); //Aleatorio en entre 1 y 2

        if (aleatorioComienzo == 1)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
        }
        else
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
        }
    }
    //Metodo que configura la interfaz de los movimientos del pokemon del jugador que este luchando con los datos de sus movimientos
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

    /// <summary>
    /// Cabecera: public void cambiarPokemon()
    /// Comentario: Este metodo se encarga de cambiar el pokemon del jugador que esta luchando, tanto si el quiere cambiarlo por otro, como si el pokemon que estaba luchando se debilito.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: El pokemon que este luchando del jugador cambiar por otro de sus pokemon, ocurriendo posteriormente dos casos:
    ///                  1: Cuando es el turno del jugador y quiere cambiar un pokemon, se realiza el cambio y se pasa al turno del rival
    ///                  2: Cuando el pokemon del jugador que estaba luchando se debilito, se realizada el cambio y se continua con el turno del jugador
    /// </summary>
    public void cambiarPokemon()
    {
        if (battleState == BattleState.PLAYERTURN || battleState == BattleState.POKEMONJUGADORDEBILITADO)
        {
            string nombreBoton = EventSystem.current.currentSelectedGameObject.transform.parent.name;
            int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);

            if (pokemonJugadorLuchando.NumeroEquipado != jugador.EquipoPokemon[numeroBotonPulsado - 1].NumeroEquipado)//Se controla que el pokemon que este luchando, no se elija otra vez para luchar
            {
                if (jugador.EquipoPokemon[numeroBotonPulsado - 1].HP > 0) //Si la vida del pokemon al que quiere cambiar en mayor que 0
                {
                    activarDesactivarMenuEquipo(true, false);
                    pokemonJugadorLuchando = jugador.EquipoPokemon[numeroBotonPulsado - 1];
                    trainerHUD.inicializarDatos(pokemonJugadorLuchando);
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
        StopCoroutine(prepararBatalla());
        StopCoroutine(atacarJugador(0));
        StopCoroutine(atacarWildPokemon());
        StopCoroutine(lanzarPokeball());
        PlayerPrefs.SetString("NameLastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).name);
        GameObject player = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        player.SetActive(true);
    }

    //Configuracion basica para cuando sea el turno de jugador
    private void turnoJugador()
    {
        battleState = BattleState.PLAYERTURN;
        textoDialogo.text = $"¿Que hara {pokemonJugadorLuchando.Nombre}?";
        activarDesactivarBotonesMenuAcciones(true);
    }
    
    /// <summary>
    /// Cabecera: public void abandonarBatallaButton()
    /// Comentario: Este metodo se encarga de iniciar la corrutina de ataque del jugador cuando sea el turno del jugador
    /// Entradas: int numeroBotonMovimiento
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Si es el turno del jugador se inicia la corrutina de ataque del jugador
    /// </summary>
    /// <param name="numeroBotonMovimiento"></param>
    public void usarMovimientoJugadorButton(int numeroBotonMovimiento)
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            StartCoroutine(atacarJugador(numeroBotonMovimiento - 1));
        }
    }
    
    /// <summary>
    /// Cabecera: IEnumerator atacarJugador(int numeroBotonPulsado)
    /// Comentario: Esta corrutina realizar la accion de ataque del pokemon de un jugador, en funcion del movimiento que se haya indicado
    /// Entradas: int numeroBotonMovimiento
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: El pokemon rival recibe daño del pokemon del juagdor luchando, posteriormente se producen dos casos:
    ///                  1: Si el pokmeon rival se debilita, la batalla termina ganando el jugador y saliendo de dicha batalla.
    ///                  2: Si el pokemon rival no se debilita, se pasa al turno del pokemon rival.           
    /// </summary>
    /// <param name="numeroBotonMovimiento"></param>
    IEnumerator atacarJugador(int numeroBotonMovimiento)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            activarDesactivarBotonesMenuAcciones(false);
            menuAtaque.SetActive(false);
            yield return new WaitForSeconds(2f); //Para que no se junte con los mensaje del enemigo, se hace una pausa y asi da tiempo de ver los mensajes de ambos
            MovimientoPokemon movimientoUsado = pokemonJugadorLuchando.Movimientos[numeroBotonMovimiento];

            int aleatorioPrecicion = UnityEngine.Random.Range(1, 100);//num aleatorio entre (1 y 100) 100 es el valor maximo que puede tener la precicion de un movimiento
            int danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad, experienciaGanada;
            bool wildPokemonVivo;
            if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, no se raliaza el ataque
            {
                //Se determina el daño del movimiento sera critico, pudiendo ser critico o no, mostrando ademas los mensajes oportunos
                danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                    PROBABILIDAD_CRITICO, pokemonJugadorLuchando.Nombre, movimientoUsado.Nombre, textoDialogo);
                yield return new WaitForSeconds(2f);
                //Se determina si habra un multiplicador por ser el movimiento efectivo contra el pokemon rival
                multiplicadorEfectividad = UtilidadesSystemaBatalla.obtenerMultiplicadorPorEfectividad(
                    wildPokemon.Debilidades, movimientoUsado.Tipo, textoDialogo);

                //Se calcula el daño final causado por el pokemon que esta luchando
                danhoPokemonCausado = UtilidadesSystemaBatalla.calcularDanhoCausado(pokemonJugadorLuchando.Nivel,
                    danhoMovimiento, multiplicadorEfectividad, pokemonJugadorLuchando.Ataque, wildPokemon.Defensa);
                
                //El pokemon rival recibe el daño y se actualiza su interfaz
                wildPokemonVivo = wildPokemon.recibirDanho(danhoPokemonCausado);
                wildPokemonHUD.setBarraSalud(wildPokemon.HP, wildPokemon.HPMaximos);
                if (!wildPokemonVivo) //Si el pokemon despues de recibir daño esta vivo
                {
                    experienciaGanada = UtilidadesSystemaBatalla.generarExperienciaDerrotarPokemonRival(wildPokemon.Nivel);
                    textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} ha ganado {experienciaGanada} de experiencia";
                    while (pokemonJugadorLuchando.comprobarSubirNivel()) //Se vuelve a comprobar con un while porque cuando sube de nivel puede ser que tenga la experiencia necesaria para subir otra vez de nivel de manera seguida
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
                textoDialogo.text = $"{pokemonJugadorLuchando.Nombre} ha fallado";
            }
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
        }
    }
    
    /// <summary>
    /// Cabecera: IEnumerator atacarWildPokemon()
    /// Comentario: Este corrutina se encarga realizar la accion de ataque del pokemon rival.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: El pokemon luchando del jugador recibe daño del pokemon , posteriormente se producen dos casos:
    ///                  1: Si el pokemon del jugador se debilita, se comprobara si el jugador cuenta con mas pokemons disponibles para seguir luchando, 
    ///                     dandose dos casos posible:
    ///                     1: Si el jugador no tiene mas pokemons para seguir luchando, la batalla termina y el jugador pierde                            
    ///                     2: Si el jugador tiene mas pokemons para seguir luchando, se pasa al turno del jugador
    ///                  2: Si el pokemon del jugador no se debilita, se pasa al turno del jugador.           
    /// </summary>
    IEnumerator atacarWildPokemon()
    {
        yield return new WaitForSeconds(2f);
        textoDialogo.text = "Es el turno del pokemon salvaje";
        yield return new WaitForSeconds(2f); //Para que no se junten los mensaje, se hace una pausa y asi da tiempo de ver los mensajes de ambos
        int aleatorioMoviminento = UnityEngine.Random.Range(0, 4),
             aleatorioPrecicion = UnityEngine.Random.Range(1, 100),danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad;
        MovimientoPokemon movimientoUsado = wildPokemon.Movimientos[aleatorioMoviminento];
        bool pokemonJugadorVivo;
        if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, pues falla
        {
            //Se determina el daño del movimiento sera critico, pudiendo ser critico o no, mostrando ademas los mensajes oportunos
            danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                PROBABILIDAD_CRITICO, wildPokemon.Nombre, movimientoUsado.Nombre, textoDialogo);
            yield return new WaitForSeconds(2f);
            //Se determina si habra un multiplicador por ser el movimiento efectivo contra el pokemon rival
            multiplicadorEfectividad = UtilidadesSystemaBatalla.obtenerMultiplicadorPorEfectividad(
                pokemonJugadorLuchando.Debilidades, movimientoUsado.Tipo, textoDialogo);
        
            //Se calcula el daño final causado por el pokemon rival 
            danhoPokemonCausado = UtilidadesSystemaBatalla.calcularDanhoCausado(wildPokemon.Nivel,
                danhoMovimiento, multiplicadorEfectividad, wildPokemon.Ataque, pokemonJugadorLuchando.Defensa);

            //El pokemon del jugador recibe el daño y se actualiza su interfaz       
            pokemonJugadorVivo = pokemonJugadorLuchando.recibirDanho(danhoPokemonCausado);
            trainerHUD.setBarraSalud(pokemonJugadorLuchando.HP, pokemonJugadorLuchando.HPMaximos);

            int numeroBotonPokemon = jugador.EquipoPokemon.IndexOf(pokemonJugadorLuchando);
            botonesPokemonsEquipo[numeroBotonPokemon].GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"PS: {pokemonJugadorLuchando.HP} / {pokemonJugadorLuchando.HPMaximos}";
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

    //Metodo que comprobara si el jugador tiene algun pokemon vivo para seguir luchando
    private bool determinarDerrotaJugador()
    {
        bool derrota = true;
        var pokemonsJugadorVivos = (from pokemon in jugador.EquipoPokemon
                                    where pokemon.HP > 0
                                    select pokemon).FirstOrDefault(); //Devolvera null si no hay ninguno.

        if (pokemonsJugadorVivos != null)
        {
            activarDesactivarMenuEquipo(false, true);
            derrota = false;
            battleState = BattleState.POKEMONJUGADORDEBILITADO;
        }
        return derrota;
    }

    //Metodo que activa el menu del equipo del jugador y bloquea el boton de salir del menu
    private void activarDesactivarMenuEquipo(bool activarBoton, bool activarMenu)
    {
        var botonAtras = menuEquipo.transform.Find("ButtonAtrasMenuEquipo");
        botonAtras.GetComponent<Button>().interactable = activarBoton;
        menuEquipo.SetActive(activarMenu);
        //c.interactable = activarBoton;
    }
    //Metodo que se encarga generar y configurar un pokemon rival de forma aleatoria
    private async Task prepararPokemonRival()
    {
        int nivelWildPokemon = UtilidadesSystemaBatalla.determinarNivelPokemonRival(jugador.EquipoPokemon);
        PokeAPI.Pokemon wildPokemonApi = await APIListadosPokemonBL.obtenerPokemonDeApi(UnityEngine.Random.Range(1, 899));
        wildPokemon = new Pokemon(wildPokemonApi);
        wildPokemon.Nivel = nivelWildPokemon;
        await wildPokemon.obtenerDatosAsincronos(wildPokemonApi);
    }
    //Metodo que bloquea los botones de la acciones principales que puede elegir un jugador en la batalla
    private void activarDesactivarBotonesMenuAcciones(bool estado)
    {
        GameObject a = GameObject.Find("ActionsZone");
        foreach (Button boton in a.GetComponentsInChildren<Button>())
        {
            boton.interactable = estado;
        }
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

        foreach (ItemConCantidad item in jugador.Mochila)
        {
            interfazItem = Instantiate(plantillaItem);
            //Imagen
            interfazItem.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Imagenes/Items/{item.Nombre}");
            //Text Nombre
            interfazItem.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = (item.CuracionPS != 0) ? $"{item.Nombre}. {item.CuracionPS}PS" : item.Nombre;
            //Text Cantidad
            interfazItem.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{item.Cantidad}";
            //Asignacion al content del scrollView
            interfazItem.transform.SetParent(contentScroView.transform);
            interfazItem.SetActive(true);
        }
    }
    /// <summary>
    /// Cabecera: public void buttonClickUsarItem(GameObject interfazItem)
    /// Comentario: Este metodo se encarga iniciar la operacion de usar un item.
    /// Entradas: GameObject interfazItem
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se guardaran los datos de la interfaz y el objeto item a usar y se realizaran dos operacion en funcion del item a usar:
    ///                  1:Si el item se trata de una pocion, se desplegara el menu del equipo del jugador.
    ///                  2:Si el item se trara de una pokeball, se realizara la accion de usar un item.
    /// </summary>
    ///<param name="interfazItem"></param>
    public void buttonClickUsarItem(GameObject interfazItem)
    {
        //Se busca cual es item que se quiere usar   
        string nombreItem = interfazItem.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
        ItemConCantidad itemUsar = jugador.Mochila.Find(g => g.Nombre == nombreItem);

        interfazItemAUsar = interfazItem; 
        itemAUsar = itemUsar; 
        if (itemUsar.Tipo == "Pocion")
        {
            configurarMenuEquipo(true);
            menuMochila.SetActive(false);
        }
        else if (itemUsar.Tipo == "Pokeball")
        {
            usarItem();
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
        for (int i = 0; i < jugador.EquipoPokemon.Count; i++)
        {
            if (activacion && jugador.EquipoPokemon[i].HP > 0  && jugador.EquipoPokemon[i].HP < jugador.EquipoPokemon[i].HPMaximos)
            {
                botonesPokemonsEquipo[i].interactable = true;
            }
            else {
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

    /// <summary>
    /// Cabecera: public void usarItem()
    /// Comentario: Este metodo se encarga de inciar la operacion correspondiente de aplicar un item y de disminuir su uso en uno. 
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran dos acciones principales en funcion del tipo del item a usar:
    ///                  1:Si el tipo del item se trara de una pocion, se iniciara una corrutina que aplicara la pocion al pokemon.
    ///                  2:Si el tipo del item se trara de una pokeball, se inciciara una corrutina asociado a la accion de una pokeball.
    /// </summary>
    public void usarItem()
    {
        menuMochila.SetActive(false); 
        activarDesactivarBotonesMenuAcciones(false);
        switch (itemAUsar.Tipo) //Necesario ya que este metodo se llamara desde el codigo y desde el inspector de unity
        {
            case "Pocion":
                StartCoroutine(aplicarPocionPokemon());
                break;

            case "Pokeball":
                StartCoroutine(lanzarPokeball());
                break;
        }
        if (--itemAUsar.Cantidad == 0)
        {
            jugador.Mochila.Remove(itemAUsar);
            Destroy(interfazItemAUsar);
        }else{
            interfazItemAUsar.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{itemAUsar.Cantidad}";
        }
    }

    /// <summary>
    /// Cabecera: IEnumerator aplicarPocionPokemon()
    /// Comentario: Esta corrutina se encarga de aplicar un item de tipo pocion a un pokemon concreto del jugador.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se restablece un numero determinado de cantidad de la vida de un pokemon especifico del jugador.
    /// </summary>
    IEnumerator aplicarPocionPokemon() 
    {
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name; //El nombre del boton corresponde a la posicion-1 de un pokemon dentro de la lista Equipo del jugador 
        int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]) - 1;
        jugador.EquipoPokemon[numeroBotonPulsado].HP += itemAUsar.CuracionPS;

        if (pokemonJugadorLuchando.Equals(jugador.EquipoPokemon[numeroBotonPulsado])) { //Si se cura el pokemon que esta luchando, para que se actualice la interfaz de la vida
            trainerHUD.setBarraSalud(pokemonJugadorLuchando.HP,pokemonJugadorLuchando.HPMaximos);
        }

        textoDialogo.text = $"Has restaurado {itemAUsar.CuracionPS}PS a {pokemonJugadorLuchando.Nombre}";
        //Se actualiza la vida de la interfaz del pokemon de ver equipo
        botonesPokemonsEquipo[numeroBotonPulsado].GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"PS: {jugador.EquipoPokemon[numeroBotonPulsado].HP} / {jugador.EquipoPokemon[numeroBotonPulsado].HPMaximos}";
        configurarMenuEquipo(false);
        yield return new WaitForSeconds(2f);
        battleState = BattleState.ENEMYTURN;
        StartCoroutine(atacarWildPokemon());
        StopCoroutine(aplicarPocionPokemon());
    }

    /// <summary>
    /// Cabecera: IEnumerator lanzarPokeball()
    /// Comentario: Esta corrutina se encarga de realizar la accion de lanzar una pokeball contra un pokemon salvaje.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realiza la accion de lanzan una pokeball, la cual puede tener dos resultado:
    ///                  1: Si el pokemon salvaje es capturado, se añade a los pokemons del jugador en la PC y finaliza la batalla
    ///                  2: Si el pokmeon salvaje no es capturado, se pasa al turno del pokemon salvaje.
    /// </summary>
    IEnumerator lanzarPokeball() {

        textoDialogo.text = $"Has lanzado una {itemAUsar.Nombre}";
        wildPokemonHUD.imagenPokemon.sprite = interfazItemAUsar.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        wildPokemonHUD.imagenPokemon.transform.localScale = new Vector3(0.35f, 0.55f, 1f);
        wildPokemonHUD.imagenPokemon.rectTransform.offsetMax = new Vector2(1.75f, -34.18f);
        yield return new WaitForSeconds(3.5f);

        bool pokemonCapturado = true;//UtilidadesSystemaBatalla.determinarCapturarPokemon(itemAUsar.IndiceExito,wildPokemon.HPMaximos,wildPokemon.HP);

        if (pokemonCapturado)
        {
            wildPokemonHUD.imagenPokemon.color = Color.grey;
            textoDialogo.text = $"{wildPokemon.Nombre} atrapado!";
            yield return new WaitForSeconds(2f);
            guardarPokemonCapturado();
            yield return new WaitForSeconds(3.5f);
            abandonarBatallaButton();
        }
        else {
            textoDialogo.text = $"Oh no el pokemon se ha escapado";
            wildPokemonHUD.imagenPokemon.transform.localScale = new Vector3(1f, 1f, 1f);
            wildPokemonHUD.imagenPokemon.sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + wildPokemon.ID).First();
            wildPokemonHUD.imagenPokemon.rectTransform.offsetMax = new Vector2(1.75f, -1.48f);
            yield return new WaitForSeconds(2f);
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
            StopCoroutine(lanzarPokeball());
        }
    }
    /// <summary>
    /// Cabecera: private void guardarPokemonCapturado()
    /// Comentario: Esta metodo se encarga de guardar en registrar el pokemon que ha capturado un jugador
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se registrara el pokemon capturado a el jugador. Se puede guardar en dos lugares:
    ///                  1:Si el jugador no tiene en su equipo 6 pokemons, el pokemon capturado se guarda en su equipo
    ///                  2:Si el jugador tiene 6 pokemons en su equipo, el pokemom capturado se guarda en el almacenamiento del PC 
    /// </summary>
    private void guardarPokemonCapturado() {
        List<PokemonJugador> totalPokemonsJugador = DatosGuardarJugador.PokemonsAlmacenadosPC.Concat(jugador.EquipoPokemon).ToList();
        int pokemonNumeroMaximo = totalPokemonsJugador.Max(g => g.PokemonNumero);
        PokemonJugador pokemonNuevo = new PokemonJugador(wildPokemon,jugador.ID,pokemonNumeroMaximo + 1,0,0);
        if (jugador.EquipoPokemon.Count < 6)
        {
            pokemonNuevo.NumeroEquipado = jugador.EquipoPokemon.Count + 1;
            jugador.EquipoPokemon.Add(pokemonNuevo);
            textoDialogo.text = $"{pokemonNuevo.Nombre} se unio al equipo!";
        }
        else
        {
            DatosGuardarJugador.PokemonsAlmacenadosPC.Add(pokemonNuevo);
            textoDialogo.text = $"{pokemonNuevo.Nombre} se almaceno en el PC!";
        }
    }
}
