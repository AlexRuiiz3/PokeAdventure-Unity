using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSystemWildPokemon : ComunBattleSystem
{
    
    async void Start()
    {
        //Se obtiene la imagen de fondo del campos de batalla(Se escogera la imagen que corresponde con la escena que esta activa)
        imagenBackGround.GetComponent<Image>().sprite = (from sprite in Resources.LoadAll<Sprite>("Imagenes/UI/EscenasBatalla/BattleBackgrounds")
                                                         where sprite.name == PlayerPrefs.GetString("EscenaAventura")
                                                         select sprite).First();
        //se busca al jugador desde resource, ya que se encuentra desabilitado
        Jugador = Resources.FindObjectsOfTypeAll<GameObject>()
                           .FirstOrDefault(g => g.CompareTag("Player"))
                           .GetComponent<PlayerController>().Jugador;


        PokemonRivalLuchando = await generarObtenerPokemonRival();
        pantallaCarga.SetActive(false);
        audio.Play();
        
        configurarMenuMochila();
        StartCoroutine(prepararIniciarBatalla());

        PokemonEncontrado pokemonEncontrado = new PokemonEncontrado(PokemonRivalLuchando.ID, PokemonRivalLuchando.Nombre);
        if (!DatosGuardarJugador.PokemonsEncontradosJugador.Exists(g => g.Id == pokemonEncontrado.Id)) {
            DatosGuardarJugador.PokemonsEncontradosJugador.Add(pokemonEncontrado);
        }
    }



    /// <summary>
    /// Cabecera:  IEnumerator prepararIniciarBatalla()
    /// Comentario: Este corrutina se encarga de configurar y preparar los campos necesarios de una batalla.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Campos de la interfaz configurados en funcion de los datos de los pokemons que se encuentren luchando.
    /// </summary>
    IEnumerator prepararIniciarBatalla()//Se hace en una corrutina para poder poner pausa y que los mensajes que se muestran no se cambien tan rapido
    {
        activarDesactivarBotonesMenuAcciones(false);
        textoDialogo.text = $"Un {PokemonRivalLuchando.Nombre} salvaje aparecio!";

        prepararConfigurarDatosJugador();
        rivalPokemonHUD.inicializarDatos(PokemonRivalLuchando);
        int aleatorioComienzo = 1;//UnityEngine.Random.Range(1, 3); //Aleatorio en entre 1 y 2

        if (aleatorioComienzo == 1)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
        }
        else
        {
            BattleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
        }
        StopCoroutine(prepararIniciarBatalla());
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
        if (BattleState == BattleState.PLAYERTURN || BattleState == BattleState.POKEMONJUGADORDEBILITADO)
        {
            string nombreBoton = EventSystem.current.currentSelectedGameObject.transform.parent.name;
            int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);

            if (PokemonJugadorLuchando.NumeroEquipado != Jugador.EquipoPokemon[numeroBotonPulsado - 1].NumeroEquipado)//Se controla que el pokemon que este luchando, no se elija otra vez para luchar
            {
                if (Jugador.EquipoPokemon[numeroBotonPulsado - 1].HP > 0) //Si la vida del pokemon al que quiere cambiar en mayor que 0
                {
                    activarDesactivarMenuEquipo(true, false);
                    PokemonJugadorLuchando = Jugador.EquipoPokemon[numeroBotonPulsado - 1];
                    trainerHUD.inicializarDatos(PokemonJugadorLuchando);
                    prepararBannerIconosMovimientos();
                    textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} te elijo a ti!";
                    if (BattleState == BattleState.PLAYERTURN)
                    {
                        activarDesactivarBotonesMenuAcciones(false);
                        BattleState = BattleState.ENEMYTURN;
                        StartCoroutine(atacarWildPokemon());
                    }
                    else//(battleState == BattleState.POKEMONJUGADORDEBILITADO)
                    {
                        turnoJugador();
                    }
                }
                else
                {
                    textoDialogo.text = $"{Jugador.EquipoPokemon[numeroBotonPulsado - 1].Nombre} no tiene fuerzas para luchar.";
                }

            }
            else
            {
                textoDialogo.text = $"{Jugador.EquipoPokemon[numeroBotonPulsado - 1].Nombre} ya esta luchando!";
            }

        }
        else
        {
            Debug.Log("No es tu turno");
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
        ItemConCantidad itemUsar = Jugador.Mochila.Find(g => g.Nombre == nombreItem);

        InterfazItemAUsar = interfazItem;
        ItemAUsar = itemUsar;
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
        switch (ItemAUsar.Tipo) //Necesario ya que este metodo se llamara desde el codigo y desde el inspector de unity
        {
            case "Pocion":
                StartCoroutine(aplicarPocionPokemon());
                break;

            case "Pokeball":
                StartCoroutine(lanzarPokeball());
                break;
        }
        if (--ItemAUsar.Cantidad == 0)
        {
            Jugador.Mochila.Remove(ItemAUsar);
            Destroy(InterfazItemAUsar);
        }
        else
        {
            InterfazItemAUsar.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{ItemAUsar.Cantidad}";
        }
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
        if (BattleState == BattleState.PLAYERTURN)
        {
            StartCoroutine(atacarJugador(numeroBotonMovimiento - 1));
        }
    }

    /// <summary>
    /// Cabecera: IEnumerator atacarJugador(int numeroBotonMovimiento)
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
            yield return new WaitForSeconds(1.5f); //Para que no se junte con los mensaje del enemigo, se hace una pausa y asi da tiempo de ver los mensajes de ambos
            MovimientoPokemon movimientoUsado = PokemonJugadorLuchando.Movimientos[numeroBotonMovimiento];

            int aleatorioPrecicion = UnityEngine.Random.Range(1, 100);//num aleatorio entre (1 y 100) 100 es el valor maximo que puede tener la precicion de un movimiento
            int danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad, experienciaGanada;
            bool wildPokemonVivo;
            if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, no se raliaza el ataque
            {
                trainerHUD.imagenPokemon.gameObject.transform.DOMove(new Vector3(trainerHUD.imagenPokemon.gameObject.transform.position.x + 20, trainerHUD.imagenPokemon.gameObject.transform.position.y + 20, 0), 0.3f);

                //Se determina el daño del movimiento sera critico, pudiendo ser critico o no, mostrando ademas los mensajes oportunos
                danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                    PROBABILIDAD_CRITICO, PokemonJugadorLuchando.Nombre, movimientoUsado.Nombre, textoDialogo);
               
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < 2; i++ ) {
                    rivalPokemonHUD.imagenPokemon.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.15f);
                    rivalPokemonHUD.imagenPokemon.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.2f);
                }

                yield return new WaitForSeconds(0.2f);
                //Se determina si habra un multiplicador por ser el movimiento efectivo contra el pokemon rival
                multiplicadorEfectividad = UtilidadesSystemaBatalla.obtenerMultiplicadorPorEfectividad(
                    PokemonRivalLuchando.Debilidades, movimientoUsado.Tipo, textoDialogo);

                //Se calcula el daño final causado por el pokemon que esta luchando
                danhoPokemonCausado = UtilidadesSystemaBatalla.calcularDanhoCausado(PokemonJugadorLuchando.Nivel,
                    danhoMovimiento, multiplicadorEfectividad, PokemonJugadorLuchando.Ataque, PokemonRivalLuchando.Defensa);

                //El pokemon rival recibe el daño y se actualiza su interfaz
                wildPokemonVivo = PokemonRivalLuchando.recibirDanho(danhoPokemonCausado);
                rivalPokemonHUD.setBarraSalud(PokemonRivalLuchando.HP, PokemonRivalLuchando.HPMaximos);
                yield return new WaitForSeconds(0.15f);
                trainerHUD.imagenPokemon.gameObject.transform.DOMove(new Vector3(trainerHUD.imagenPokemon.gameObject.transform.position.x - 20, trainerHUD.imagenPokemon.gameObject.transform.position.y - 20, 0), 0.3f);
                yield return new WaitForSeconds(2f);
                if (!wildPokemonVivo) //Si el pokemon despues de recibir daño no esta vivo
                {
                    rivalPokemonHUD.imagenPokemon.enabled = true;
                    audio.Pause();
                    audio.clip = Resources.Load<AudioClip>("Audio/Batalla/VictoryBattle");
                    audio.Play();
                    textoDialogo.text = $"¡El {PokemonRivalLuchando.Nombre} enemigo se debilito!";
                    yield return new WaitForSeconds(2f);
                    experienciaGanada = UtilidadesSystemaBatalla.generarExperienciaDerrotarPokemonRival(PokemonRivalLuchando.Nivel);
                    textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} ha ganado {experienciaGanada} puntos de experiencia.";
                    while (PokemonJugadorLuchando.comprobarSubirNivel()) //Se vuelve a comprobar con un while porque cuando sube de nivel puede ser que tenga la experiencia necesaria para subir otra vez de nivel de manera seguida
                    {
                        yield return new WaitForSeconds(3.5f);
                        trainerHUD.setTextNivel(PokemonJugadorLuchando.Nivel);
                        UtilidadesEscena.llamarActivarAudioMomentaneo("Batalla/LevelUp", 1f);
                        textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} ha subido al nivel {PokemonJugadorLuchando.Nivel}!";
                    }
                    yield return new WaitForSeconds(3f);
                    textoDialogo.text = "¡Victoria! Saliendo del combate...";
                    BattleState = BattleState.WIN;
                    yield return new WaitForSeconds(4f);
                    abandonarBatallaButton();//Aqui se abandonara la corrutina, la escena
                }
            }
            else
            {
                yield return new WaitForSeconds(2f);
                textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} uso {movimientoUsado.Nombre}, pero ha fallado!";
            }
            BattleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
            StopCoroutine(atacarJugador(0));
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
        textoDialogo.text = "Es el turno del pokemon salvaje!";
        yield return new WaitForSeconds(3f); //Para que no se junten los mensaje, se hace una pausa y asi da tiempo de ver los mensajes de ambos
        int aleatorioMoviminento = UnityEngine.Random.Range(0, PokemonRivalLuchando.Movimientos.Count),
             aleatorioPrecicion = UnityEngine.Random.Range(1, 100),danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad;
        MovimientoPokemon movimientoUsado = PokemonRivalLuchando.Movimientos[aleatorioMoviminento];
        bool pokemonJugadorVivo;
        if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, pues falla
        {
            rivalPokemonHUD.imagenPokemon.gameObject.transform.DOMove(new Vector3(rivalPokemonHUD.imagenPokemon.gameObject.transform.position.x - 20, rivalPokemonHUD.imagenPokemon.gameObject.transform.position.y - 20, 0), 0.3f);
            //Se determina el daño del movimiento sera critico, pudiendo ser critico o no, mostrando ademas los mensajes oportunos
            danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                PROBABILIDAD_CRITICO, PokemonRivalLuchando.Nombre, movimientoUsado.Nombre, textoDialogo);

            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 2; i++)
            {
                trainerHUD.imagenPokemon.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.15f);
                trainerHUD.imagenPokemon.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.2f);
            //Se determina si habra un multiplicador por ser el movimiento efectivo contra el pokemon rival
            multiplicadorEfectividad = UtilidadesSystemaBatalla.obtenerMultiplicadorPorEfectividad(
                PokemonJugadorLuchando.Debilidades, movimientoUsado.Tipo, textoDialogo);
        
            //Se calcula el daño final causado por el pokemon rival 
            danhoPokemonCausado = UtilidadesSystemaBatalla.calcularDanhoCausado(PokemonRivalLuchando.Nivel,
                danhoMovimiento, multiplicadorEfectividad, PokemonRivalLuchando.Ataque, PokemonJugadorLuchando.Defensa);

            //El pokemon del jugador recibe el daño y se actualiza su interfaz       
            pokemonJugadorVivo = PokemonJugadorLuchando.recibirDanho(danhoPokemonCausado);
            trainerHUD.setBarraSalud(PokemonJugadorLuchando.HP, PokemonJugadorLuchando.HPMaximos);

            yield return new WaitForSeconds(0.15f);
            rivalPokemonHUD.imagenPokemon.gameObject.transform.DOMove(new Vector3(rivalPokemonHUD.imagenPokemon.gameObject.transform.position.x + 20, rivalPokemonHUD.imagenPokemon.gameObject.transform.position.y + 20, 0), 0.3f);

            int numeroBotonPokemon = Jugador.EquipoPokemon.IndexOf(PokemonJugadorLuchando);
            botonesPokemonsEquipo[numeroBotonPokemon].GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"PS: {PokemonJugadorLuchando.HP} / {PokemonJugadorLuchando.HPMaximos}";
           
            if (!pokemonJugadorVivo) //Si el pokemon despues de recibir daño no esta vivo
            {
                prepararIconosPokemosDisponibles(Jugador.EquipoPokemon.Cast<Pokemon>().ToList(), trainerHUD.pokemonsDisponibles);
                yield return new WaitForSeconds(2f);
                textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} se ha debilitado!";
                if (determinarDerrotaJugador())
                {
                    yield return new WaitForSeconds(2f);
                    textoDialogo.text = "No te quedan pokemons para seguir luchando.";
                    yield return new WaitForSeconds(2.5f);
                    textoDialogo.text = "Abandonas el combate para ir a curar a tu equipo.";
                    yield return new WaitForSeconds(2.5f);
                    textoDialogo.text = "Con las prisas se te caen unas monedas...";
                    BattleState = BattleState.LOST;
                    yield return new WaitForSeconds(2f);
                    configurarDerrotaJugador();
                }
                else
                {                   
                    yield return new WaitForSeconds(1.5f);
                    textoDialogo.text = "¡Elige un pokemon para luchar!";
                    StopCoroutine(atacarWildPokemon());
                }
            }
        }
        else
        {
            textoDialogo.text = $"El {PokemonRivalLuchando.Nombre} salvaje ha fallado!";
        }
        if (BattleState == BattleState.ENEMYTURN)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
             StopCoroutine(atacarWildPokemon());
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
    public IEnumerator aplicarPocionPokemon()
    {
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name; //El nombre del boton corresponde a la posicion-1 de un pokemon dentro de la lista Equipo del jugador 
        int numeroBotonPulsado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]) - 1;
        Jugador.EquipoPokemon[numeroBotonPulsado].HP += ItemAUsar.CuracionPS;

        UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/UsarPocion", 1f);

        if (PokemonJugadorLuchando.Equals(Jugador.EquipoPokemon[numeroBotonPulsado]))
        { //Si se cura el pokemon que esta luchando, para que se actualice la interfaz de la vida
            trainerHUD.setBarraSalud(PokemonJugadorLuchando.HP, PokemonJugadorLuchando.HPMaximos);
        }

        textoDialogo.text = $"Has restaurado {ItemAUsar.CuracionPS}PS a {PokemonJugadorLuchando.Nombre}.";
        //Se actualiza la vida de la interfaz del pokemon de ver equipo
        botonesPokemonsEquipo[numeroBotonPulsado].GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"PS: {Jugador.EquipoPokemon[numeroBotonPulsado].HP} / {Jugador.EquipoPokemon[numeroBotonPulsado].HPMaximos}";
        configurarMenuEquipo(false);
        yield return new WaitForSeconds(2f);
        BattleState = BattleState.ENEMYTURN;
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

        textoDialogo.text = $"Has lanzado una {ItemAUsar.Nombre}!";
        rivalPokemonHUD.imagenPokemon.sprite = InterfazItemAUsar.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        rivalPokemonHUD.imagenPokemon.transform.localScale = new Vector3(0.35f, 0.52f, 1f);
        rivalPokemonHUD.imagenPokemon.rectTransform.offsetMax = new Vector2(1.75f, -34.18f);
        bool pokemonCapturado = UtilidadesSystemaBatalla.determinarCapturarPokemon(ItemAUsar.IndiceExito,PokemonRivalLuchando.HPMaximos, PokemonRivalLuchando.HP);
        if (pokemonCapturado) {
            UtilidadesEscena.llamarActivarAudioMomentaneo("Batalla/PokemonCapturado", 3.5f);
        }
        else {
            UtilidadesEscena.llamarActivarAudioMomentaneo("Batalla/PokemonNoCapturado", 3.5f);
        }

        yield return new WaitForSeconds(3.25f);

        if (pokemonCapturado)
        {
            UtilidadesEscena.activarPausarMusicaEscenaActiva(false);
            UtilidadesEscena.activarMusicaTemporal("Batallas/GetPokemon", true);
            rivalPokemonHUD.imagenPokemon.color = Color.grey;
            textoDialogo.text = $"{PokemonRivalLuchando.Nombre} atrapado!";
            yield return new WaitForSeconds(2.5f);
            guardarPokemonCapturado();
            yield return new WaitForSeconds(3.5f);
            textoDialogo.text = $"Saliendo del combate.";
            yield return new WaitForSeconds(4.5f);
            abandonarBatallaButton();
        }
        else {
            textoDialogo.text = $"Oh no el pokemon se ha escapado.";
            rivalPokemonHUD.imagenPokemon.transform.localScale = new Vector3(1f, 1f, 1f);
            rivalPokemonHUD.imagenPokemon.sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + PokemonRivalLuchando.ID).First();
            rivalPokemonHUD.imagenPokemon.rectTransform.offsetMax = new Vector2(1.75f, -1.48f);
            yield return new WaitForSeconds(2f);
            BattleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarWildPokemon());
        }
        StopCoroutine(lanzarPokeball());
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
        List<PokemonJugador> totalPokemonsJugador = DatosGuardarJugador.PokemonsAlmacenadosPC.Concat(Jugador.EquipoPokemon).ToList();
        int pokemonNumeroMaximo = totalPokemonsJugador.Max(g => g.PokemonNumero);
        PokemonJugador pokemonNuevo = new PokemonJugador(PokemonRivalLuchando,Jugador.ID,pokemonNumeroMaximo + 1,0,0,PokemonRivalLuchando.Nivel * 50);
        if (Jugador.EquipoPokemon.Count < 6)
        {
            pokemonNuevo.NumeroEquipado = Jugador.EquipoPokemon.Count + 1;
            Jugador.EquipoPokemon.Add(pokemonNuevo);
            textoDialogo.text = $"{pokemonNuevo.Nombre} se unio al equipo!";
        }
        else
        {
            DatosGuardarJugador.PokemonsAlmacenadosPC.Add(pokemonNuevo);
            textoDialogo.text = $"{pokemonNuevo.Nombre} se almaceno en el PC!";
        }
    }
}
