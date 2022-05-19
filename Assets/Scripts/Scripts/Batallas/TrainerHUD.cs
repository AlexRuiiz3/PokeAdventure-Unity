using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class TrainerHUD : MonoBehaviour
{
    public Text nombrePokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public GameObject pokemonsDisponibles;
    public Sprite spritePokeball;
    public Image imagenPokemon;

    public void inicializarDatos(PokemonJugador pokemon) {
        nombrePokemon.text = pokemon.Nombre.Substring(0, 1).ToUpper() + pokemon.Nombre.Substring(1);
        setBarraSalud(pokemon.HP, pokemon.HPMaximos);
        setTextNivel(pokemon.Nivel);
        imagenPokemon.sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Back/" + pokemon.ID).First();
        
    }
    /// <summary>
    /// Metodo que modifica de la caja HP del usuario los campos del nombre del pokemon 
    /// </summary>
    /// <param name="pokemon"></param>
    public void setBarraSalud(int hp, int hpMaximos) {
        UtilidadesEscena.modificarBarraSalud(barraSalud, hp, hpMaximos);
    }
    public void setTextNivel(int nivel) {
        nivelPokemon.text = $"Lv{nivel}";
    }
    public void prepararIconosPokemosDisponibles(int numeroPokemonsDisponibles) {
        GameObject gameObjectImagen;
        Image imagen;
        //RectTransfor almacena la posicion, tamaño, anclaje y pivote de una rectangulo. En este caso el gameObject pokemonsDisponibles es un rectangulo(Donde se almacenaran las imagenes)
        RectTransform rt = pokemonsDisponibles.GetComponent<RectTransform>();
        //Un gameObject solo puede tener un objeto grafico, por eso en el for se tiene que crear un nuevo gameObject y asignarle la imagen
        for (int i = 0; i < numeroPokemonsDisponibles; i++)
        {
            gameObjectImagen = new GameObject();

            imagen = gameObjectImagen.AddComponent<Image>(); //Ua nueva imagen que se añadira automaticamente al gameObject(gameObjectImagen)
            imagen.sprite = spritePokeball;
            //De esta forma se mofidica el ancho y alto de la imagen 
            imagen.rectTransform.sizeDelta = new Vector2(20f, 20f);

            //Al nuevo gameObject con la imagen su transform(x,y,z) padre se modifica para que sea el del gameObject pokemonsDisponibles, asi la imagen se incluira en el gameObject pokemonsDisponibles
            //Al indicarle false es como decirle que se ponga dentro/junto de rt(rt es el recuadro del gameObject pokemonDisponibles) que pasara a ser su padre ahora, True indicara que tome la posicion global de la escena.
            gameObjectImagen.transform.SetParent(rt, false);
        }
    }
}
