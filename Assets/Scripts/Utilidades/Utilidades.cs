using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Utilidades
{
    /// <summary>
    /// Cabecera: public static bool comprobarCadenaVacia(string cadena)
    /// Comentario: Este metodo se encarga de comprobar si la cadena rebidida como parametros se encuentra vacia.
    /// Entradas: string cadena
    /// Salidas: bool
    /// Precondiciones: cadena debe ser distinto de null.
    /// Postcondiciones: Se devolvera un booleano que puede tomar dos posibles valores:
    ///                  true: Si la cadena recibida esta vacia.
    ///                  false: Si la cadena recibida no esta vacia.
    ///                  Si se recibide una cadena que este a null, se producira un NullPointerException.
    /// </summary>
    ///<param name="cadena"></param>
    public static bool comprobarCadenaVacia(string cadena)
    {
        return string.IsNullOrEmpty(cadena.Trim());
    }

    /// <summary>
    /// Cabecera: public static byte[] obtenerImagenDeUrl(string url)
    /// Comentario: Este metodo se encarga de obtener una imagen de una url como byte[]
    /// Entradas: string url
    /// Salidas: byte[] foto
    /// Precondiciones: url debera de ser una url correcta.
    /// Postcondiciones: Se obtendra una imagen de una url como byte[].
    /// </summary>
    ///<param name="url"></param>
    public static byte[] obtenerImagenDeUrl(string url)
    {
        byte[] imagen;
        using (WebClient webClient = new WebClient())
        {
            imagen = webClient.DownloadData(url);
        }
        return imagen;
    }

    /// <summary>
    /// Cabecera: public static Sprite convertirArrayBytesASprite(byte[] imagen)
    /// Comentario: Este metodo se encarga de convertir un byte[] en un Sprite
    /// Entradas: byte[] imagen
    /// Salidas: Sprite
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se obtendra un Sprite a partir de un byte[].
    /// </summary>
    ///<param name="imagen"></param>
    public static Sprite convertirArrayBytesASprite(byte[] imagen)
    {
        Texture2D texture = new Texture2D(1, 1); //Texture2D necesario, porque sera donde se carge la imagen como byte[]
        texture.LoadImage(imagen);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
    }

    public static void cambiarPrimerCaracterMayuscula(string cadena)
    {
        cadena = cadena.Substring(0, 1).ToUpper() + cadena.Substring(1);
    }

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
    /// Cabecera: public static List<string> obtenerFrasesAleatoriasTrainer()
    /// Comentario: Este metodo se encarga de obtener una listado de string de forma aleatoria.
    /// Entradas: Ninguna
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se obtendra un List<string>
    /// </summary>
    public static List<string> obtenerFrasesAleatoriasTrainer() { 
    List<List<string>> frasesEntrenadores = new List<List<string>> { 
        new List<string> { "Hola viajero, veo que tienes un buen equipo.", "Dejame que compruebe si es verdad." },
        new List<string> { "¡Eh! ¡Tu equipo Pokémon me da buenas vibraciones! ", "¿Te importa que vea lo entrenado que está?" },
        new List<string> { "Te voy hacer comer polvo.", "Para que te hagas una idea, como el que va a comer Santi con el proyecto final." },
        new List<string> { "¿Qué necesita mi vista?"},
        new List<string> { "F1", "." },
        new List<string> { "F2", "." },
        new List<string> { "F3", "." },
        new List<string> { "F4", "." },
        new List<string> { "F5", "." },
        new List<string> { "F6", "." },
        new List<string> { "F7", "." },
        new List<string> { "F8", "." }
    };
        return frasesEntrenadores[Random.Range(0, frasesEntrenadores.Count)];
    }

    /// <summary>
    /// Cabecera: public static List<string> obtenerFrasesAleatoriasTrainerDerrotado()
    /// Comentario: Este metodo se encarga de obtener una listado de string de forma aleatoria.
    /// Entradas: Ninguna
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se obtendra un List<string>
    /// </summary>
    public static List<string> obtenerFrasesAleatoriasTrainerDerrotado()
    {
        List<List<string>> frasesEntrenadoresDerotao = new List<List<string>> {
        new List<string> { "Pues si que eres fuerte."},
        new List<string> { "Pufff que combate más bueno, he aprendido mucho.", "Espero que volvamos a combatir otra vez." },
        new List<string> { "Que sepas que me he dejado.", "No quiero abusar de un crio." },
        new List<string> { "...","..."},
        new List<string> { "FD1", "." },
        new List<string> { "FD2", "." },
        new List<string> { "FD3", "." },
        new List<string> { "FD4", "." },
        new List<string> { "FD5", "." },
        new List<string> { "FD6", "." },
        new List<string> { "FD7", "." },
        new List<string> { "FD8", "." }
    };
        return frasesEntrenadoresDerotao[Random.Range(0, frasesEntrenadoresDerotao.Count)];
    }
}
