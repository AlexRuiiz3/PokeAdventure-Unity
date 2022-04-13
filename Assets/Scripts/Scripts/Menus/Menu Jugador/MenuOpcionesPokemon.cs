using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpcionesPokemon : MonoBehaviour
{
    public PokemonJugador Pokemon { get; set; }
    public Text textNombrePokemon;
    void Start()
    {
    }

    public void cambiarTextoTextNombrePokemon() {
        textNombrePokemon.text = Pokemon.Nombre;
    }
    public void verDatos()
    {
        Debug.Log("Ver Datos");
    }
    public void cambiarNombre()
    {
        Debug.Log("Cambiar Nombre");
    }
    public void cambiarPosicion()
    {
        Debug.Log("Cambiar Posicion");
    }
}
