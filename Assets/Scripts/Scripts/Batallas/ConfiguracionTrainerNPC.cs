using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionTrainerNPC : MonoBehaviour
{
    public Text nombrePokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public GameObject pokemonsDisponibles;
    public Sprite spritePokeball;
    public Image imagenPokemon;
    private TrainerNPC trainerNPC;

    void Start()
    {
        prepararIconosPokemosDisponibles();

    }
    /// <summary>
    /// Metodo que modifica de la caja HP del usuario los campos del nombre del pokemon 
    /// </summary>
    /// <param name="pokemon"></param>
    public void modificarDatos(Pokemon pokemon)
    {
        nombrePokemon.text = pokemon.Nombre;
        nivelPokemon.text = $"Lv{pokemon.Nivel}";
        barraSalud.transform.localScale = new Vector3((pokemon.HP / pokemon.HPMaximos), 1f);
    }
    private void prepararIconosPokemosDisponibles() 
    {
        GameObject gameObjectImagen;
        Image imagen;
        //RectTransfor almacena la posicion, tamaño, anclaje y pivote de una rectangulo. En este caso el gameObject pokemonsDisponibles es un rectangulo(Donde se almacenaran las imagenes)
        RectTransform rt = pokemonsDisponibles.GetComponent<RectTransform>();
        //Un gameObject solo puede tener un objeto grafico, por eso en el for se tiene que crear un nuevo gameObject y asignarle la imagen
        for (int i = 0; i < 2; i++)
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
