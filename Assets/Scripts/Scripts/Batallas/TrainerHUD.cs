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
        UtilidadesSystemaBatalla.modificarBarraSalud(barraSalud, hp, hpMaximos);
    }
    public void setTextNivel(int nivel) {
        nivelPokemon.text = $"Lv{nivel}";
    }

}
