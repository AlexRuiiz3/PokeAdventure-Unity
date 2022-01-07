using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionWildPokemon : MonoBehaviour
{
    public Text nombrePokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public Image imagenPokemon;

     void Start()
    {
        Pokemon pokemon = null;
        nombrePokemon.text = "Evee";//pokemon.Nombre;
        nivelPokemon.text = $"Lv{100}";//$"Lv{pokemon.Nivel}";
        barraSalud.transform.localScale = new Vector3(1f, 1f,1f); //El primero es 1f, porque la barra de vida de un pokemon salvaje la primera vez siempre estara completa
    }
    public void setBarraSalud(Pokemon pokemon) {
        barraSalud.transform.localScale = new Vector3((pokemon.HP / pokemon.HPMaximos),1f);
    }
}
