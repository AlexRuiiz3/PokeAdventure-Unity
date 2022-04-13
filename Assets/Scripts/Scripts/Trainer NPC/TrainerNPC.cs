using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerNPC : MonoBehaviour
{
    //private bool derrotado = false;

    //Constructor sin parametros
    public TrainerNPC() {
        Frase = "";
        EquipoPokemon = new List<Pokemon>();
        Mochila = new List<Item>();
    }
    //Constructor con parametros
    public TrainerNPC(string frase, List<Pokemon> equipoPokemon, List<Item> mochila)
    {
        Frase = frase;
        EquipoPokemon = equipoPokemon;
        Mochila = mochila;
    }


    #region Metodos fundamentales(Propiedades)
    //Frase
    public string Frase { get; set; }
    //EquipoPokemon
    public List<Pokemon> EquipoPokemon { get; }
    //Mochila
    public List<Item> Mochila { get; set; }
    #endregion

    
}
