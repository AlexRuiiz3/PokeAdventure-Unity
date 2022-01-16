using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpcionPokemons : MonoBehaviour
{
    public PlayerController scriptPlayer;//Creo que tiene que ser GameObject
    public List<Button> botonesPokemons;
    public GameObject menuOpcionesPokemon;
    public Text textNumeroPokemons;
    private int numeroPokemonSeleccionado;

    void Start()
    {

        gameObject.SetActive(false); //Se desactiva el menu

        /*
        //Se desactivan todos los botones, y luego debajo se activan los necesarios
        foreach (Button button in botonesPokemons) {
            button.gameObject.SetActive(false);
        }*/
        List<PokemonJugador> aaaa = new List<PokemonJugador>();

        aaaa.Add(new PokemonJugador(new Pokemon(2, "Pikachu", 100, 20, 3, 4, 5, null, null, null, ListadosPokemon.getImageFrentePokemon(44), null), 10, 1, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Ratta", 50, 12, 3, 4, 5, null, null, null, ListadosPokemon.getImageFrentePokemon(21), null), 10, 2, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Squirtle", 35, 8, 3, 4, 5, null, null, null, ListadosPokemon.getImageFrentePokemon(105), null), 10, 3, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Zubat", 120, 54, 3, 4, 5, null, null, null, ListadosPokemon.getImageFrentePokemon(88), null), 10, 4, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Mew", 325, 100, 3, 4, 5, null, null, null, ListadosPokemon.getImageFrentePokemon(122), null), 10, 5, 1, 1));

        textNumeroPokemons.text = $"Equipo Actual {aaaa.Count}/6";

        Image imagenPokemon; Texture2D texture; //Texture2D necesario, porque sera donde se carge el la imagen del pokemon como byte[]
        Text textNombrePokemon;
        Text textHPPokemon;
        Text textNivelPokemon;

        PokemonJugador pokemon;
        List<Component> componentesBoton = new List<Component>();

        for(int i = 0; i < aaaa.Count; i++) {
            //for (int i = 0; i < scriptPlayer.Jugador.EquipoPokemon.Count; i++)
           // {
            pokemon = aaaa[i];

            botonesPokemons[i].gameObject.SetActive(true);
            /* Se obtienen todos los componentes del boton, sus hijos seran el Image y los 3 Text que se esta buscando. 
             * Para encontrarlos al boton hay que llamar al metodo GetComponentsInChildren que devolvera todos los compoentes 
             * hijos que tiene el boton y los atributos de estos.
             * Hay que hacer el foreach porque GetComponentsInChildren contiene todos los atributos de los 
             * componentes(De la imagen y los text se coge CavasRenderer,RectTransform y Image o Text) y lo que 
             * se necesita para modificar son el atributo Image o Text de los respectivos componetes Image o Text.
             */
            foreach (Component componente in botonesPokemons[i].GetComponentsInChildren<Component>()) {
                if (componente is Text || componente is Image) {
                    componentesBoton.Add(componente);
                }
            }

            imagenPokemon = (Image)componentesBoton[1]; //No se tiene encuenta la posicion 0 porque en esa esta la imagen del propio boton(Boton en el que se encuentra la Image y los 3 Text)
            texture = new Texture2D(1, 1);
            texture.LoadImage(pokemon.ImagenDeFrente);
            imagenPokemon.sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(texture.width/2, texture.height/2));
           
            textNombrePokemon = (Text)componentesBoton[2];
            textNombrePokemon.text = $"Nombre: {pokemon.Nombre}";

            textHPPokemon = (Text)componentesBoton[3];
            textHPPokemon.text = $"PS: {pokemon.HP} / {pokemon.HPMaximos}";

            textNivelPokemon = (Text)componentesBoton[4];
            textNivelPokemon.text = $"Nvl. {pokemon.Nivel}";

            botonesPokemons[i].onClick.AddListener(verOpcionesPokemon);

            componentesBoton.Clear(); //Se limpia la lista con los componentes del boton para que despues guardar los componentes del siguiente boton y asi las posicion 1,2,3,4 corresponderan a los componentes del boton que le toque en la iteracion
        }


    }
    public void verOpcionesPokemon()
    {
        List<PokemonJugador> aaaa = new List<PokemonJugador>();

        aaaa.Add(new PokemonJugador(new Pokemon(2, "Pikachu", 100, 20, 3, 4, 5, null, null, null, null, null), 10, 1, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Ratta", 50, 12, 3, 4, 5, null, null, null, null, null), 10, 2, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Squirtle", 35, 8, 3, 4, 5, null, null, null, null, null), 10, 3, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Zubat", 120, 54, 3, 4, 5, null, null, null, null, null), 10, 4, 1, 1));
        aaaa.Add(new PokemonJugador(new Pokemon(2, "Mew", 325, 100, 3, 4, 5, null, null, null, null, null), 10, 5, 1, 1));



        /* EventSystem.current.currentSelectedGameObject.name me dara el nombre del GameObject seleccionado, En este caso el de un boton,
         * como los botones su nombre es ButtonPokemon 1, ButtonPokemon 2, ect. Se coge el ultimo caracter del nombre que 
         * corresponde al numero del boton clicado 
         */
        string nombreBoton = EventSystem.current.currentSelectedGameObject.name;
        int numeroBotonClicado = (int)char.GetNumericValue(nombreBoton[nombreBoton.Length - 1]);

        menuOpcionesPokemon.GetComponent<MenuOpcionesPokemon>().Pokemon = aaaa[numeroBotonClicado - 1];//scriptPlayer.Jugador.EquipoPokemon[numeroPokemon
        menuOpcionesPokemon.GetComponent<MenuOpcionesPokemon>().cambiarTextoTextNombrePokemon(); //Mejor hacerlo aqui porque es mas optimo, la otra forma seria en la clase MenuOpcionesPokemon hacer el cambio de nombre en un metodo update y eso no es lo mas optimo
        menuOpcionesPokemon.SetActive(true);
        
    }

    /*
     * Se hace asi porque delegate { verOpcionesPokemon(pokemon.PokemonNumero); }
     * 
     * De esta manera no se le puede incluir parametros por eso poner lo de delegate
     * botonesPokemons[0].onClick.AddListener(verOpcionesPokemon(pokemon.PokemonNumero))
     */
}

