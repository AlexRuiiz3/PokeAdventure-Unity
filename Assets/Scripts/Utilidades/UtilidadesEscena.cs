using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Cinemachine;

/* Clase con metodos que realizan alguna iteracion con los elementos de una escena, como activar o desactivar algo, 
 * o mostrar o eliminar algun elemento de una escena.
 */
public class UtilidadesEscena : MonoBehaviour
{

    private void Start() 
    {
        GameObject camara = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == $"CM"+SceneManager.GetActiveScene().name);

        if (camara != null)
        {
            GameObject player = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g =>  g.CompareTag("Player"));
            camara.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        }
    }

    /// <summary>
    /// Cabecera: public static void precargarEscena(string escenaSiguiente)
    /// Comentario: Este metodo se encarga de guardar cual es la siguiente escena a cargar y cargar una escena de cargando.
    /// Entradas: string escenaSiguiente
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se cargara una escena de cargando.
    /// <param name="escenaSiguiente"></param>
    /// </summary>
    public static void precargarEscena(string escenaSiguiente)
    {
        PlayerPrefs.SetString("NameNextScene", escenaSiguiente);
        SceneManager.LoadScene("LoadingScene");
    }
    /// <summary>
    /// Cabecera: public static void eliminarGameObjectsItemsYEntrenadores()
    /// Comentario: Este metodo se encarga de eliminar todos los gameObjects que tengan el tag Trainer y los que tengan el tag Item 
    /// Entradas: Ninguna
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se destruiran los gameObjects con el tag Trainer o Item
    /// </summary>
    public static void eliminarGameObjectsItemsYEntrenadores()
    {
        GameObject[] trainersYItems = GameObject.FindGameObjectsWithTag("Trainer");

        trainersYItems = trainersYItems.Concat(GameObject.FindGameObjectsWithTag("Item")).ToArray();
        foreach (GameObject gameObject in trainersYItems)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Cabecera: public static void activarDesactivarIteracionBotones(List<Button> botones, bool modo)
    /// Comentario: Este metodo se encarga de activar o desactivar la iteracion de una lista de botones que recibe.
    /// Entradas: List<Button> botones, bool modo
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran sobre los botone la operacion que detemine el parametero booleano(modo) recibido:
    ///                  true: Activara la iteracion de los botones de la lista de botones.
    ///                  false: Desactivara la iteracion de los botones de la lista de botones.
    /// </summary>
    /// <param name="botones"></param>
    /// <param name="modo"></param>
    public static void activarDesactivarIteracionBotones(List<Button> botones, bool modo)
    {
        foreach (Button boton in botones)
        {
            boton.interactable = modo;
        }
    }

    /// <summary>
    /// Cabecera: public static void cerrarMenus(GameObject menu)
    /// Comentario: Este metodo se encarga desactivar todos los objetos menus. 
    /// Entradas: GameObject menu
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se cargara una escena de cargando.
    /// <param name="menu"></param>
    /// </summary>
    public static void cerrarMenus(GameObject menu) {
        List<GameObject> subMenus = obtenerMenusHijosActivados(menu);
        activarDesactivarGameObjects(subMenus,false);
    }

    //Metodo que se encarga de obtener todos los GameObject hijos que sean un menu de manera recursiva(De un hijo se obtendran sus hijos que sean un menu, etc)
    private static List<GameObject> obtenerMenusHijosActivados(GameObject menu) {
        List<GameObject> subMenus = new List<GameObject>();
        foreach (Transform child in menu.transform)
        {
            if (child.gameObject.name.Contains("Menu") || child.gameObject.name.Contains("MensajeError") && child.gameObject.activeSelf)
            {
                subMenus = subMenus.Concat(obtenerMenusHijosActivados(child.gameObject)).ToList();
            }
        }
        subMenus.Add(menu);
        return subMenus;
    }
    /// <summary>
    /// Cabecera: public static void activarDesactivarBotones(List<Button> botones, bool modo)
    /// Comentario: Este metodo se encarga de activar o desactivar una lista de botones que recibe.
    /// Entradas: List<Button> botones, bool modo
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran sobre los botone la operacion que detemine el parametero booleano(modo) recibido:
    ///                  true: Activara los botones de la lista de botones.
    ///                  false: Desactivara los botones de la lista de botones.
    /// </summary>
    /// <param name="botones"></param>
    /// <param name="modo"></param>
    public static void activarDesactivarGameObjects(List<GameObject> gameObjects, bool modo)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(modo);
        }
    }
    /// <summary>
    /// Cabecera: public static void activarDesactivarMenuYTiempoJuego(GameObject menu)
    /// Comentario: Este metodo se encarga de activar o desactivar un gameObject que sera un menu de la interfaz de usuario y de activar o pausara el tiempo del juego.
    /// Entradas: GameObject menu
    /// Salidas: Niguna
    /// Precondiciones: menu no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se realizara alguna de las siguientes operaciones en funcion de si el menu que se recibe se encuentra activo o no:
    ///                  1: Si el menu recibido ya esta activado, se reactivara el tiempo de juego y se desactivara el menu.
    ///                  2: Si el menu recibido no esta activado, se pausara el tiempo de juego y se activara el menu.
    /// </summary>
    /// <param name="menu"></param>
    public static void activarDesactivarMenuYTiempoJuego(GameObject menu)
    {
        if (menu.activeSelf)
        {
            Time.timeScale = 1f; //El tiempo del juego se reactiva
            menu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f; //El tiempo del juego pasa a ser 0 por que se pausa el juego 
            menu.SetActive(true);
        }
    }

    public void cerrarJuego() {
        Application.Quit();
    }
    /// <summary>
    /// Cabecera: public static void activarPausarMusicaEscenaActiva(bool modo)
    /// Comentario: Este metodo se encarga de pausar la musica que tenga la escena que se encuentra activa.
    /// Entradas: bool modo
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se parara la musica que tiene la escena que se encuentre activa. Si la escena no tiene musica, no se pausara la musica.
    /// <param name="modo"></param>
    /// </summary>
    public static void activarPausarMusicaEscenaActiva(bool modo)
    {
        bool encontrado = false;
        GameObject[] gameObjectsEscena = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < gameObjectsEscena.Length && !encontrado; i++)
        {
            if (gameObjectsEscena[i].name == "Grid" || SceneManager.GetActiveScene().name.Contains("Battle"))
            {
                encontrado = true;
                if (modo)
                {
                    gameObjectsEscena[i].GetComponent<AudioSource>().Play();
                }
                else
                {
                    gameObjectsEscena[i].GetComponent<AudioSource>().Pause();
                }
            }
        }
    }
    
    /// <summary>
    /// Cabecera: public static void destruirGameObjectEspecifico(string nombre)
    /// Comentario: Este metodo se encarga destruir un GameObject en especifico
    /// Entradas: string nombre
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se destruira un GameObject determinado. Si el GameObject que se desea eliminar no existe o no se encuentra, no se eliminara
    /// <param name="nombre"></param>
    /// </summary>
    public static void destruirGameObjectEspecifico(string nombre)
    {
        GameObject objetoDestruir = GameObject.Find(nombre);
        if(objetoDestruir != null)
        {
        Destroy(objetoDestruir);
        }    
    }
    
    /// <summary>
    /// Cabecera: public static void activarMusicaTemporal(string musicaActivar, bool enBucle)
    /// Comentario: Este metodo se encarga de activar una musica temporalmente.
    /// Entradas: string musicaActivar, bool enBucle
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se iniciara un audio temporal.
    /// <param name="musicaActivar"></param>
    /// <param name="enBucle"></param>
    /// </summary>
    public static void activarMusicaTemporal(string musicaActivar, bool enBucle)
    {
        GameObject audioTemporal = new GameObject();
        audioTemporal.transform.position = new Vector3(0, 0, 0);
        audioTemporal.name = "AudioTemporal";
        AudioSource audioSource = audioTemporal.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>($"Audio/{musicaActivar}");
        audioSource.loop = enBucle;
        audioSource.Play();
    }

    /// <summary>
    /// Cabecera: public static void activarAudioMomentaneo(string musica, float duracion)
    /// Comentario: Este metodo se encarga de realizar la llamada para iniciar la activacion de un audio momentaneo durante unos segundos.
    /// Entradas: string musica, float duracion
    /// Salidas: Niguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se iniciara un audio momentaneo unos segundos.
    /// <param name="musica"></param>
    /// <param name="duracion"></param>
    /// </summary>
    public static void llamarActivarAudioMomentaneo(string musica, float duracion)//Necesario para acceder a el desde unity
    { 
        GameObject.Find("Utilidades").GetComponent<UtilidadesEscena>().activarAudioMomentaneo(musica, duracion);
    }
    private void activarAudioMomentaneo(string musica, float duracion) { //Una corrutina no se puede llamar en un metodo static por eso se usa este metodo
        StartCoroutine(activarMusica(musica,duracion));
    }

    //Corrutina que activa la musica y la desactiva despues de los segundos que se haya indicado por parametro
    public IEnumerator activarMusica(string musica, float duracion)
    {
        GameObject audioTemporal = new GameObject();
        audioTemporal.transform.position = new Vector3(0, 0, 0);
        audioTemporal.name = "AudioMomentaneo";
        AudioSource audioSource = audioTemporal.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>($"Audio/{musica}");
        audioSource.Play();
        yield return new WaitForSeconds(duracion);
        Destroy(audioTemporal);
        StopCoroutine(activarMusica("", 0));
    }
    /// <summary>
    /// Cabecera: public static void mostrarMensajeError(string mensaje) 
    /// Comentario: Este metodo se encarga de mostrar en un menu personalizado el mensaje recibo.
    /// Entradas: string mensaje
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se mostrara un menu personalizado que contendra el mensaje recibido como parametro.
    /// </summary>
    /// <param name="mensaje"></param>
    public static void mostrarMensajeError(string mensaje)
    {
        GameObject menuError;
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            menuError = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault<GameObject>(g => g.name == "MensajeErrorMainScene");
        }
        else
        {
            menuError = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault<GameObject>(g => g.name == "MenuMensajeError");
        }
        menuError.GetComponentsInChildren<TextMeshProUGUI>()[1].text = mensaje;
        menuError.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public static void eliminarHijosGameObject(GameObject gameObject)
    /// Comentario: Este metodo se encarga de eliminar todos los hijos que tenga un gameObject.
    /// Entradas: GameObject gameObject 
    /// Salidas: Ninguna
    /// Precondiciones: object no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se eliminaran todos los hijos de un gameObject.
    /// <param name="gameObject"></param>
    /// </summary>
    public static void eliminarHijosGameObject(GameObject gameObject)
    {
        if (gameObject.transform.childCount > 0)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
    /// <summary>
    /// Cabecera: public static void prepararBotonesPokemonsEquipo(List<PokemonJugador> pokemonsJugador, List<Button> botones)
    /// Comentario: Este metodo se encarga de configurar y preparar una lista de botones que hacen referencia a los pokemons del jugador en el menu de equipo.
    /// Entradas: List<PokemonJugador> pokemonsJugador, List<Button> botones
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se configurar los botones recibidos por parametro.
    /// </summary>
    /// <param name="pokemonsJugador"></param>
    /// <param name="botones"></param>
    public static void prepararBotonesPokemonsEquipo(List<PokemonJugador> pokemonsJugador, List<Button> botones)
    {
        Image imagenPokemon;
        TextMeshProUGUI textNombrePokemon, textHPPokemon, textNivelPokemon;
        PokemonJugador pokemon;
        List<Component> componentesBoton = new List<Component>();

        for (int i = 0; i < pokemonsJugador.Count; i++) //Por cada pokemon que tenga el jugador se activa y prepara un boton
        {
            //for (int i = 0; i < scriptPlayer.Jugador.EquipoPokemon.Count; i++)
            pokemon = pokemonsJugador[i];

            botones[i].gameObject.SetActive(true);
            botones[i].gameObject.name = pokemonsJugador[i].PokemonNumero.ToString();
            /* Se obtienen todos los componentes del boton, sus hijos seran el Image y los 3 Text que se esta buscando. 
             * Para encontrarlos al boton hay que llamar al metodo GetComponentsInChildren que devolvera todos los compoentes 
             * hijos que tiene el boton y los atributos de estos.
             * Hay que hacer el foreach porque GetComponentsInChildren contiene todos los atributos de los 
             * componentes(De la imagen y los text se coge CavasRenderer,RectTransform y Image o Text) y lo que 
             * se necesita para modificar son el atributo Image o Text de los respectivos componetes Image o Text.
             */
            foreach (Component componente in botones[i].GetComponentsInChildren<Component>())
            {
                if (componente is TextMeshProUGUI || componente is Image)
                {
                    componentesBoton.Add(componente);
                }
            }

            imagenPokemon = (Image)componentesBoton[1]; //No se tiene encuenta la posicion 0 porque en esa esta la imagen del propio boton(Boton en el que se encuentra la Image y los 3 Text)

            imagenPokemon.sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();

            textNombrePokemon = (TextMeshProUGUI)componentesBoton[2];
            textNombrePokemon.text = pokemon.Nombre;

            textHPPokemon = (TextMeshProUGUI)componentesBoton[3];
            textHPPokemon.text = $"PS: {pokemon.HP} / {pokemon.HPMaximos}";

            textNivelPokemon = (TextMeshProUGUI)componentesBoton[4];
            textNivelPokemon.text = $"Nvl. {pokemon.Nivel}";

            componentesBoton.Clear(); //Se limpia la lista con los componentes del boton para que despues guardar los componentes del siguiente boton y asi las posicion 1,2,3,4 corresponderan a los componentes del boton que le toque en la iteracion
        }
    }

    /// <summary>
    /// Cabecera: public static void configurarMenuDatosPokemon(GameObject menuDatos, PokemonJugador pokemon)
    /// Comentario: Este metodo se encarga de configurar y activar el menu con los datos de un pokemon.
    /// Entradas: GameObject menuDatos, PokemonJugador pokemon
    /// Salidas: Ninguna
    /// Precondiciones: menuDatos y pokemon no deben estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: El menu de datos esta configurado y activado con los valores del pokemon.
    /// </summary>
    /// <param name="menuDatos"></param>
    /// <param name="pokemon"></param>
    public static void configurarMenuDatosPokemon(GameObject menuDatos, PokemonJugador pokemon)
    {
        GameObject imagenTipo2 = menuDatos.transform.GetChild(4).gameObject;
        //Se prepara el menu con la informacion del pokemon seleccionado    
        menuDatos.GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemon.Nombre;
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"N:{pokemon.ID}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Nvl.{pokemon.Nivel}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"Experiencia: {pokemon.Experiencia} Exp";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[4].text = $"Siguiente nivel: {pokemon.ExperienciaSiguienteNivel} Exp";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[5].text = $"Ataque: {pokemon.Ataque}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[6].text = $"Defensa: {pokemon.Defensa}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[7].text = $"Velocidad: {pokemon.Velocidad}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[8].text = $"HP: {pokemon.HP}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[9].text = $"HPMax: {pokemon.HPMaximos}";

        menuDatos.GetComponentsInChildren<Image>()[3].sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/IconosNombre/" + pokemon.Tipos[0]).First();
        if (pokemon.Tipos.Count > 1)
        {
            imagenTipo2.SetActive(true);
            imagenTipo2.GetComponentInChildren<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/IconosNombre/" + pokemon.Tipos[1]).First();
        }
        else {
            imagenTipo2.SetActive(false);
        }
        menuDatos.SetActive(true);
    }
    /// <summary>
    /// Cabecera: public static void configurarMostrarMenuMovimientos(GameObject menuMovimientos, PokemonJugador pokemon)
    /// Comentario: Este metodo se encarga de configurar y mostrar el menu de movimientos que tiene un pokemon.
    /// Entradas: GameObject menuMovimientos, PokemonJugador pokemon
    /// Salidas: Ninguna
    /// Precondiciones: menuMovimientos y pokemon no deben estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: El menu de movimientos esta configurado y activado con los valores del pokemon.
    /// </summary>
    /// <param name="menuMovimientos"></param>
    /// <param name="pokemon"></param>
    public static void configurarMostrarMenuMovimientos(GameObject menuMovimientos, PokemonJugador pokemon) {
        GameObject movimientoInterfaz;
        MovimientoPokemon movimientoPokemon;
        for (int i = 1; i < menuMovimientos.transform.childCount - 1; i++)
        { //Empieza en 1 porque es hijo 0 no es un movimiento sino un titulo
            movimientoPokemon = pokemon.Movimientos[i - 1];
            movimientoInterfaz = menuMovimientos.transform.GetChild(i).gameObject;
            movimientoInterfaz.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/Banners/" + movimientoPokemon.Tipo).First();
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[0].text = movimientoPokemon.Nombre;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Potencia: {movimientoPokemon.Danho}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Presicion: {movimientoPokemon.Precicion}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"PP {movimientoPokemon.PP}/{movimientoPokemon.PPMaximo}";
        }
        menuMovimientos.SetActive(true);
    }
}
