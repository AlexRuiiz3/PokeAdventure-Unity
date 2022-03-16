using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WildPokemonHUD : MonoBehaviour
{
    public Text nombrePokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public Image imagenPokemon;

    public void inicializarDatos(Pokemon pokemon)
    {
        nombrePokemon.text = pokemon.Nombre;
        nivelPokemon.text = $"Lv{pokemon.Nivel}";
        barraSalud.transform.localScale = new Vector3(1f, 1f, 1f); //El primero es 1f, porque la barra de vida de un pokemon salvaje la primera vez siempre estara completa
        if (pokemon.ImagenDeFrente != null)
        {
            imagenPokemon.sprite = Utilidades.convertirArrayBytesASprite(pokemon.ImagenDeFrente);
        }
    }
    public void setBarraSalud(Pokemon pokemon) {
        barraSalud.transform.localScale = new Vector3((float)pokemon.HP / pokemon.HPMaximos,1f,1f);
    }
}
