using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

/* Clase con metodos que realizan alguna iteracion con los elementos de una escena, como activar o desactivar algo, 
 * o mostrar o eliminar algun elemento de una escena.
 */
public class UtilidadesEscena : MonoBehaviour
{
    public static void precargarEscena(string escenaSiguiente)
    {
        GameObject jugador = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        DontDestroyOnLoad(jugador);
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
        trainersYItems.Concat(GameObject.FindGameObjectsWithTag("Item"));

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

    /// <summary>
    /// Cabecera: public static void pausarMusicaEscenaActiva()
    /// Comentario: Este metodo se encarga de pausar la musica que tenga la escena que se encuentra activa.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se parara la musica que tiene la escena que se encuentre activa. Si la escena no tiene musica, no se pausara la musica.
    /// </summary>
    public static void activarPausarMusicaEscenaActiva(bool modo)
    {
        bool encontrado = false;
        GameObject[] gameObjectsEscena = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < gameObjectsEscena.Length && !encontrado; i++)
        {
            if (gameObjectsEscena[i].name == "Grid" || gameObjectsEscena[i].name == "Battle")
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
    public static void destruirGameObjectEspecifico(string nombre) {
        Destroy(GameObject.Find(nombre));
    }
    public static void activarMusicaTemporal(string musicaActivar, bool enBucle)
    {
        GameObject audioTemporal = new GameObject();
        audioTemporal.transform.position = new Vector3(0,0,0);
        audioTemporal.name = "AudioTemporal";
        AudioSource audioSource = audioTemporal.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>($"Audio/{musicaActivar}");
        audioSource.loop = enBucle;
        audioSource.Play();
    }
    public static void llamarActivarAudioMomentaneo(string musica,float duracion) { //Engloba la llamada al metodo que inicia la musica momentanea
        GameObject.Find("Utilidades").GetComponent<UtilidadesEscena>().activarAudioMomentaneo(musica, duracion);
    }
    public void activarAudioMomentaneo(string musica, float duracion)//Necesario para la corrutina
    {
        StartCoroutine(activarMusica(musica, duracion));
    }
    IEnumerator activarMusica(string musica, float duracion) {
        
        GameObject audioTemporal = new GameObject();
        audioTemporal.transform.position = new Vector3(0, 0, 0);
        audioTemporal.name = "AudioMomentaneo";
        AudioSource audioSource = audioTemporal.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>($"Audio/{musica}");
        audioSource.Play();
        yield return new WaitForSeconds(duracion);
        Destroy(audioTemporal);
        StopCoroutine(activarMusica("",0));
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
        GameObject menuError = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MensajeError");
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
    /// </summary>
    /// <param name="gameObject"></param>
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
    /// Cabecera: public static void configurarMenuDatosPokemon(GameObject menuDatos, PokemonJugador pokemon)
    /// Comentario: Este metodo se encarga de configurar y activar el menu con los datos de un pokemon.
    /// Entradas: GameObject menuDatos, PokemonJugador pokemon
    /// Salidas: Ninguna
    /// Precondiciones: menuDatos y pokemon no deben estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: El menu de datos esta configurado y activado con los valores del pokemon seleccionado
    /// </summary>
    /// <param name="menuDatos"></param>
    /// <param name="pokemon"></param>
    public static void configurarMenuDatosPokemon(GameObject menuDatos, PokemonJugador pokemon)
    {
        //Se prepara el menu con la informacion del pokemon seleccionado    
        menuDatos.GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemon.Nombre;
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Nvl.{pokemon.Nivel}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"PS: {pokemon.HP}/{pokemon.HPMaximos}";

        //Se prepara la parte de los movimientos del pokemon 
        GameObject movimientos = menuDatos.transform.Find("Movimientos").gameObject, movimientoInterfaz;
        MovimientoPokemon movimientoPokemon;
        for (int i = 1; i < movimientos.transform.childCount; i++)
        { //Empieza en 1 porque es hijo 0 no es un movimiento sino un titulo
            movimientoPokemon = pokemon.Movimientos[i - 1];
            movimientoInterfaz = movimientos.transform.GetChild(i).gameObject;
            movimientoInterfaz.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Imagenes/UI/Tipos/Banners/" + movimientoPokemon.Tipo)[0]; ;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[0].text = movimientoPokemon.Nombre;
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Potencia: {movimientoPokemon.Danho}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Presicion: {movimientoPokemon.Precicion}";
            movimientoInterfaz.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"PP {movimientoPokemon.PP}/{movimientoPokemon.PPMaximo}";
        }
        menuDatos.SetActive(true);
    }
}
