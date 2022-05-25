using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Clase con metodos que realizan alguna iteracion con los elementos de una escena, como activar o desactivar algo, 
 * o mostrar o eliminar algun elemento de una escena.
 */
public class UtilidadesEscena : MonoBehaviour
{
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
    public static void activarDesactivarBotones(List<Button> botones, bool modo)
    {
        foreach (Button boton in botones)
        {
            boton.gameObject.SetActive(modo);
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
    public static void activarDesactivarMenuYTiempoJuego(GameObject menu) {
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
    public static void pausarMusicaEscenaActiva()
    {
        bool encontrado = false;
        GameObject[] gameObjectsEscena = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < gameObjectsEscena.Length && !encontrado; i++)
        {
            if (gameObjectsEscena[i].name == "Grid")
            {
                encontrado = true;
                gameObjectsEscena[i].GetComponent<AudioSource>().Stop();
                gameObjectsEscena[i].GetComponent<AudioSource>().enabled = false;
                gameObjectsEscena[i].GetComponent<AudioListener>().enabled = false;
            }
        }
    }

    //Creo que este metodo no es necesario, no se usa
    public static void activarMusicaEscenaActiva()
    {
        bool encontrado = false;
        GameObject[] gameObjectsEscena = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < gameObjectsEscena.Length && !encontrado; i++)
        {
            if (gameObjectsEscena[i].name == "Grid")
            {
                encontrado = true;
                gameObjectsEscena[i].GetComponent<AudioSource>().Play();
                gameObjectsEscena[i].GetComponent<AudioSource>().enabled = true;
                gameObjectsEscena[i].GetComponent<AudioListener>().enabled = true;
            }
        }
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
    /// Cabecera: public static void modificarBarraSalud(Image barraSalud, int hp, int hpMaximos)
    /// Comentario: Este metodo se encarga de modificar la imagen que representa la vida de un pokemon en funcion de la vida que tenga este.
    /// Entradas: Image barraSalud, int hp, int hpMaximos
    /// Salidas: Ninguna
    /// Precondiciones: barraSalud no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se modificara la imagen de vida de un pokemon, cambiando el color de esta en funcion de la vida que tenga el pokemon.
    /// </summary>
    /// <param name="barraSalud"></param>
    /// <param name="hp"></param>
    /// <param name="hpMaximos"></param>
    public static void modificarBarraSalud(Image barraSalud, int hp, int hpMaximos) {
        barraSalud.transform.localScale = new Vector3((float)hp / hpMaximos, 1f, 1f);

        if (barraSalud.transform.localScale.x >= 0.5f)
        {
            barraSalud.color = new Color32(0, 255, 106,255);//Verde
        }
        else if (barraSalud.transform.localScale.x < 0.15f)
        {
            barraSalud.color = Color.red;
        }
        else {
            barraSalud.color = Color.yellow;
        }
    }

    /// <summary>
    /// Cabecera: public static void eliminarHijosGameObject(GameObject contentScrollView)
    /// Comentario: Este metodo se encarga de eliminar todos los hijos que tenga un gameObject.
    /// Entradas: GameObject object 
    /// Salidas: Ninguna
    /// Precondiciones: object no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se eliminaran todos los hijos de un gameObject.
    /// </summary>
    /// <param name="object"></param>
    public static void eliminarHijosGameObject(GameObject object)
    {
        if (object.transform.childCount > 0)
        {
            foreach (Transform child in object.transform)
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
        menuDatos.GetComponentsInChildren<Image>()[1].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemonSeleccionado.ID).First();
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[0].text = pokemonSeleccionado.Nombre;
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Nvl.{pokemonSeleccionado.Nivel}";
        menuDatos.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"PS: {pokemonSeleccionado.HP}/{pokemonSeleccionado.HPMaximos}";

        //Se prepara la parte de los movimientos del pokemon 
        GameObject movimientos = menuDatos.transform.Find("Movimientos").gameObject, movimientoInterfaz;
        MovimientoPokemon movimientoPokemon;
        for (int i = 1; i < movimientos.transform.childCount; i++)
        { //Empieza en 1 porque es hijo 0 no es un movimiento sino un titulo
            movimientoPokemon = pokemonSeleccionado.Movimientos[i - 1];
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
