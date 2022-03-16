using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtenerPrimerPokemon : MonoBehaviour
{
    public GameObject[] menusPokemons;

    void Start()
    {
        ocultarMenusPokemons();
        FindObjectOfType<ControlDialogos>().activarDialogo();
        FindObjectOfType<ControlDialogos>().siguienteFrase();
    }

    private void ocultarMenusPokemons()
    {
        foreach (GameObject menu in menusPokemons)
        {
            menu.SetActive(false);
        }
    }
}
