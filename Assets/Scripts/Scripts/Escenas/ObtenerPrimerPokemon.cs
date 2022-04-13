using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtenerPrimerPokemon : MonoBehaviour
{
    public GameObject[] menusPokemons;
    public List<Button> botonesPokemos;
    public bool botonesActivados = false;

    void Start()
    {
        UtilidadesEscena.activarDesactivarIteracionBotones(botonesPokemos,false);
        FindObjectOfType<ControlDialogos>().activarDialogo();
    }

    private void Update()
    {
        if (!botonesActivados && PlayerPrefs.GetString("EstadoDialogo") == "Terminado") {
            UtilidadesEscena.activarDesactivarIteracionBotones(botonesPokemos, true);
            botonesActivados = true; 
        }
    }
}
