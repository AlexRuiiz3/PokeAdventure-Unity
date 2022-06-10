using DG.Tweening;
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

public class BattleSystemPokemonTrainer : ComunBattleSystem
{
    private TrainerNPC trainerNPC;
    async void Start()
    {
        trainerNPC = DatosGenerales.trainerLuchando;
        rivalPokemonHUD.imagenRival.sprite = trainerNPC.Imagen;
        //Se obtiene la imagen de fondo del campos de batalla(Se escogera la imagen que corresponde con la escena que esta activa) 
        imagenBackGround.GetComponent<Image>().sprite = (from sprite in Resources.LoadAll<Sprite>("Imagenes/UI/EscenasBatalla/BattleBackgrounds")
                                                         where sprite.name == PlayerPrefs.GetString("EscenaAventura")
                                                         select sprite).First();
        //se busca al jugador desde resource, ya que se encuentra desabilitado
        Jugador = Resources.FindObjectsOfTypeAll<GameObject>()
                           .FirstOrDefault(g => g.CompareTag("Player"))
                           .GetComponent<PlayerController>().Jugador;

        await prepararEntrenadorRival();
        pantallaCarga.SetActive(false);
        audio.clip = Resources.Load<AudioClip>($"Audio/Batalla/BattleTrainer{UnityEngine.Random.Range(1, 5)}");
        audio.Play();

        configurarMenuMochila();
        StartCoroutine(prepararIniciarBatalla());
    }

    /// <summary>
    /// Cabecera:  IEnumerator prepararBatalla()
    /// Comentario: Este corrutina se encarga de configurar y preparar los campos necesarios de una batalla.
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Campos de la interfaz configurados en funcion de los datos de los pokemons que se encuentren luchando.
    /// </summary>
    IEnumerator prepararIniciarBatalla()//Se hace en una corrutina para poder poner pausa y que los mensajes que se muestran no se cambien tan rapido
    {
        //activarDesactivarBotonesMenuAcciones(false);
        textoDialogo.text = $"Entrenador rival te desafia!";
        //Preparacion Jugador
        prepararConfigurarDatosJugador();

        //Preparacion trainer rival
        PokemonRivalLuchando = trainerNPC.EquipoPokemon[0];
        rivalPokemonHUD.inicializarDatos(PokemonRivalLuchando);
        prepararIconosPokemosDisponibles(trainerNPC.EquipoPokemon.Cast<Pokemon>().ToList(), rivalPokemonHUD.pokemonsDisponibles);


        int aleatorioComienzo = UnityEngine.Random.Range(1, 3); //Aleatorio en entre 1 y 2

        if (aleatorioComienzo == 1)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
        }
        else
        {
            activarDesactivarBotonesMenuAcciones(false);
            BattleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarEntranadorRival());
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
                        StartCoroutine(atacarEntranadorRival());
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
            yield return new WaitForSeconds(1.5f); //Para que no se junte con los mensaje del enemigo, se hace una pausa y asi da tiempo de ver los mensajes de ambos
            MovimientoPokemon movimientoUsado = PokemonJugadorLuchando.Movimientos[numeroBotonMovimiento];

            int aleatorioPrecicion = UnityEngine.Random.Range(1, 100);//num aleatorio entre (1 y 100) 100 es el valor maximo que puede tener la precicion de un movimiento
            int danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad, experienciaGanada;
            bool rivalPokemonVivo;
            if (aleatorioPrecicion <= movimientoUsado.Precicion)//Si precicion(60 <= 90(Precicion del movimiento)) se ataca, si es mayor que 90 que es la precicion del movimiento, no se raliaza el ataque
            {
                trainerHUD.imagenPokemon.gameObject.transform.DOMove(new Vector3(trainerHUD.imagenPokemon.gameObject.transform.position.x + 20, trainerHUD.imagenPokemon.gameObject.transform.position.y + 20, 0), 0.3f);

                //Se determina el daño del movimiento sera critico, pudiendo ser critico o no, mostrando ademas los mensajes oportunos
                danhoMovimiento = UtilidadesSystemaBatalla.incrementarDanhoMovimientoPorCritico(movimientoUsado.Danho,
                    PROBABILIDAD_CRITICO, PokemonJugadorLuchando.Nombre, movimientoUsado.Nombre, textoDialogo);

                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < 2; i++)
                {
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
                rivalPokemonVivo = PokemonRivalLuchando.recibirDanho(danhoPokemonCausado);
                rivalPokemonHUD.setBarraSalud(PokemonRivalLuchando.HP, PokemonRivalLuchando.HPMaximos);
                yield return new WaitForSeconds(0.15f);
                trainerHUD.imagenPokemon.gameObject.transform.DOMove(new Vector3(trainerHUD.imagenPokemon.gameObject.transform.position.x - 20, trainerHUD.imagenPokemon.gameObject.transform.position.y - 20, 0), 0.3f);
                yield return new WaitForSeconds(2f);
                if (!rivalPokemonVivo) //Si el pokemon despues de recibir daño esta vivo
                {
                    rivalPokemonHUD.imagenPokemon.enabled = false;
                    prepararIconosPokemosDisponibles(trainerNPC.EquipoPokemon, rivalPokemonHUD.pokemonsDisponibles);
                    textoDialogo.text = $"¡{PokemonRivalLuchando.Nombre} enemigo se debilito!";
                    experienciaGanada = UtilidadesSystemaBatalla.generarExperienciaDerrotarPokemonRival(PokemonRivalLuchando.Nivel);
                    yield return new WaitForSeconds(3f);
                    textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} ha ganado {experienciaGanada} de experiencia.";
                    while (PokemonJugadorLuchando.comprobarSubirNivel()) //Se vuelve a comprobar con un while porque cuando sube de nivel puede ser que tenga la experiencia necesaria para subir otra vez de nivel de manera seguida
                    {
                        yield return new WaitForSeconds(3.5f);
                        trainerHUD.setTextNivel(PokemonJugadorLuchando.Nivel);
                        UtilidadesEscena.llamarActivarAudioMomentaneo("Batalla/LevelUp", 1f);
                        textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} ha subido al nivel {PokemonJugadorLuchando.Nivel}!";
                    }

                    if (determinarDerrotaRival())
                    {
                        audio.Pause();
                        audio.clip = Resources.Load<AudioClip>("Audio/Batalla/VictoryBattle");
                        audio.Play();
                        trainerNPC.derrotado = true;
                        yield return new WaitForSeconds(3.5f);
                        Jugador.Dinero += trainerNPC.dineroAlDerrotar;
                        textoDialogo.text = $"Has derrotado al entrenador rival y obtenido {trainerNPC.dineroAlDerrotar}$!";
                        yield return new WaitForSeconds(3.5f);
                        textoDialogo.text = "¡Victoria! Saliendo del combate...";
                        BattleState = BattleState.WIN;
                        yield return new WaitForSeconds(4f);
                        abandonarBatallaButton();//Aqui se abandonara la corrutina, la escena
                    }
                    else {
                        yield return new WaitForSeconds(3f);
                        PokemonRivalLuchando = trainerNPC.EquipoPokemon.First(g => g.HP > 0);
                        rivalPokemonHUD.inicializarDatos(PokemonRivalLuchando);
                        rivalPokemonHUD.imagenPokemon.enabled = true;
                        textoDialogo.text = $"El entrendador rival saca a {PokemonRivalLuchando.Nombre}.";
                        yield return new WaitForSeconds(3f);
                    }
                }
            }
            else
            {
                textoDialogo.text = $"{PokemonJugadorLuchando.Nombre} ha fallado!";
            }
            BattleState = BattleState.ENEMYTURN;
            StartCoroutine(atacarEntranadorRival());
            StopCoroutine(atacarJugador(0));
        }
    }
    /// <summary>
    /// Cabecera: IEnumerator atacarEntranadorRival()
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
    IEnumerator atacarEntranadorRival()
    {
        yield return new WaitForSeconds(2f);
        textoDialogo.text = $"Es el turno del {PokemonRivalLuchando.Nombre} rival!";
        yield return new WaitForSeconds(3f); //Para que no se junten los mensaje, se hace una pausa y asi da tiempo de ver los mensajes de ambos

        if (!determinarRivalUsaCuracion())
        { //Si no usa una curacion
            int aleatorioMoviminento = UnityEngine.Random.Range(0, PokemonRivalLuchando.Movimientos.Count),
                 aleatorioPrecicion = UnityEngine.Random.Range(1, 100), danhoMovimiento, danhoPokemonCausado, multiplicadorEfectividad;
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
                    textoDialogo.text = $"¡{PokemonJugadorLuchando.Nombre} se debilito!";
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
                        yield return new WaitForSeconds(1f);
                        textoDialogo.text = "¡Elige un pokemon para luchar!";
                        StopCoroutine(atacarEntranadorRival());
                    }
                }
            }
            else
            {
                textoDialogo.text = $"El {PokemonRivalLuchando.Nombre} salvaje ha fallado!";
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            usarItemRival();
        }

        if (BattleState == BattleState.ENEMYTURN)
        {
            yield return new WaitForSeconds(2f);
            turnoJugador();
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
        StartCoroutine(atacarEntranadorRival());
        StopCoroutine(aplicarPocionPokemon());
    }

    private bool determinarDerrotaRival()
    {
        bool derrota = true;
        if (UtilidadesSystemaBatalla.comprobarPokemonsVivos(trainerNPC.EquipoPokemon))
        {
            derrota = false;
        }
        return derrota;
    }

    private bool determinarRivalUsaCuracion()
    {
        return trainerNPC.Mochila.Count > 0 && trainerNPC.EquipoPokemon.Exists(g => g.HP > 0 && ((float)g.HP / g.HPMaximos) <= 0.5);
    }

    private void usarItemRival()
    {
        int randomItemUsar = UnityEngine.Random.Range(0, trainerNPC.Mochila.Count);
        ItemConCantidad itemUsar = trainerNPC.Mochila[randomItemUsar];
        UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/UsarPocion", 1f);
        textoDialogo.text = $"El entrenador rival uso {itemUsar.Nombre}.";
        PokemonRivalLuchando.HP += itemUsar.CuracionPS;
        rivalPokemonHUD.setBarraSalud(PokemonRivalLuchando.HP, PokemonRivalLuchando.HPMaximos);

        if (--itemUsar.Cantidad < 1)
        {
            trainerNPC.Mochila.Remove(itemUsar);
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
    }

    /// <summary>
    /// Cabecera: public void usarItem()
    /// Comentario: Este metodo se encarga de inciar la operacion correspondiente de aplicar un item y de disminuir su uso en uno. 
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizara la siguiente accion:
    ///                  1:Si el tipo del item se trara de una pocion, se iniciara una corrutina que aplicara la pocion al pokemon.
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
        }
        if (--ItemAUsar.Cantidad == 0)
        {
            Jugador.Mochila.Remove(ItemAUsar);
            Destroy(InterfazItemAUsar);
            if (Jugador.Mochila.Count <= 0) {
                textSinItems.SetActive(true);
            }
        }
        else
        {
            InterfazItemAUsar.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{ItemAUsar.Cantidad}";
        }
    }

    private async Task<List<Pokemon>> generarEquipoPokemonRival()
    {
        int numeroPokemons = generarCantidadPokemonsEquipo();
        List<Pokemon> pokemons = new List<Pokemon>();
        for (int i = 0; i < numeroPokemons; i++)
        {
            pokemons.Add(await generarObtenerPokemonRival());
        }
        return pokemons;
    }

    private int generarCantidadPokemonsEquipo()
    {
        int pokemonsEquipoRival = 2;
        /*
        bool randomValido = false;
        while (!randomValido)
        { //Para que sea mas dificil que se genere un equipo de 5 o 6 pokemon, ya que seria mas complicado de derrotar
            pokemonsEquipoRival = UnityEngine.Random.Range(1, 7);
            if (pokemonsEquipoRival == 5 || pokemonsEquipoRival == 6)
            {
                if (UnityEngine.Random.Range(1, 5) == 1)
                { //25% de que se de como valido el numero 5 y 6
                    randomValido = true;
                }
            }
            else
            {
                randomValido = true;
            }
        }*/
        return pokemonsEquipoRival;
    }
    private List<ItemConCantidad> generarItemsEntrenadorRival(int numeroItems)
    {
        List<ItemConCantidad> itemsEntrenador = new List<ItemConCantidad>();
        List<Item> items = ListadosItemBL.obtenerItems();
        items = items.FindAll(g => g.Tipo == "Pocion");
        ItemConCantidad item;
        int randomIndexItem, cantidadItems;
        while (itemsEntrenador.Count < numeroItems)
        {
            randomIndexItem = UnityEngine.Random.Range(1, items.Count);
            cantidadItems = UnityEngine.Random.Range(1, 4);
            item = new ItemConCantidad(items.ElementAt(randomIndexItem), cantidadItems);
            if (!itemsEntrenador.Exists(g => g.Equals(item)))
            {
                itemsEntrenador.Add(item);
            }
        }
        return itemsEntrenador;
    }
    private async Task prepararEntrenadorRival()
    {
        int numeroItems = UnityEngine.Random.Range(0, 2);
        if (numeroItems > 0)
        {
            trainerNPC.Mochila = generarItemsEntrenadorRival(numeroItems);
        }
        trainerNPC.EquipoPokemon = await generarEquipoPokemonRival();
        trainerNPC.dineroAlDerrotar = UnityEngine.Random.Range(100, 151) * trainerNPC.EquipoPokemon.Count;
    }
}
