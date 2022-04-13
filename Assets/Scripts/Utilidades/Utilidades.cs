using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Utilidades
{
    public static bool comprobarCadenaVacia(string cadena)
    {
        return string.IsNullOrEmpty(cadena.Trim());
    }

    public static byte[] obtenerImagenDeUrl(string url)
    {
        byte[] foto;
        using (WebClient webClient = new WebClient())
        {
            foto = webClient.DownloadData(url);
        }
        return foto;
    }

    public static Sprite convertirArrayBytesASprite(byte[] imagen)
    {
        Texture2D texture = new Texture2D(1, 1); //Texture2D necesario, porque sera donde se carge el la imagen del pokemon como byte[]
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

        foreach (Button button in botones) //Primero se desactivan todos
        {
            button.gameObject.SetActive(false);
        }

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
            if (pokemon.ImagenDeFrente != null)
            {
                imagenPokemon.sprite = Utilidades.convertirArrayBytesASprite(pokemon.ImagenDeFrente);
            }

            textNombrePokemon = (TextMeshProUGUI)componentesBoton[2];
            textNombrePokemon.text = pokemon.Nombre;

            textHPPokemon = (TextMeshProUGUI)componentesBoton[3];
            textHPPokemon.text = $"PS: {pokemon.HP} / {pokemon.HPMaximos}";

            textNivelPokemon = (TextMeshProUGUI)componentesBoton[4];
            textNivelPokemon.text = $"Nvl. {pokemon.Nivel}";

            componentesBoton.Clear(); //Se limpia la lista con los componentes del boton para que despues guardar los componentes del siguiente boton y asi las posicion 1,2,3,4 corresponderan a los componentes del boton que le toque en la iteracion
        }
    }
}
