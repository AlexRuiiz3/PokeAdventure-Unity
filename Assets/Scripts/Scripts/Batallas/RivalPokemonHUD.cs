using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RivalPokemonHUD : MonoBehaviour
{
    public Text nombrePokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public Image imagenPokemon;
    public GameObject pokemonsDisponibles;//Sera solo para cuando se enfrente a un entrenador, contra pokemons salvajes no se usara

    public void inicializarDatos(Pokemon pokemon)
    {
        nombrePokemon.text = pokemon.Nombre;
        nivelPokemon.text = $"Lv{pokemon.Nivel}";
        setBarraSalud(pokemon.HP, pokemon.HPMaximos);
        imagenPokemon.sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
        
    }
    public void setBarraSalud(int hp, int hpMaximos) {
        UtilidadesEscena.modificarBarraSalud(barraSalud, hp, hpMaximos);
    }
}
