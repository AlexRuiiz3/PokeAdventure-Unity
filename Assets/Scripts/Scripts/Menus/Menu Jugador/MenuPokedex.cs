using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPokedex : MonoBehaviour
{

    private Jugador jugador;
    public void activarMenuPokedex() {
        GameObject menuPokedex = gameObject; //gameObject hace referencia a el gameObject que tendra el script en este caso menuPokedex
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador; 
        //Text de Menu Pokedex
        menuPokedex.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Pokemons encontrados {DatosGuardarJugador.PokemonsEncontradosJugador.Count}/898";
        menuPokedex.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Pokemons atrapados {jugador.EquipoPokemon.Count + DatosGuardarJugador.PokemonsAlmacenadosPC.Count}";

        menuPokedex.SetActive(true);
    }

    public void configurarPrepararMenuPokemonsGeneracion(string limitesPokemonGeneracion) {
        GameObject scrollViewPokemons = gameObject.transform.Find("ScrollViewPokemonsGeneraciones").gameObject;
        scrollViewPokemons.GetComponentInChildren<Scrollbar>().value = 1;
        
        GameObject content = scrollViewPokemons.transform.GetChild(0).Find("Content").gameObject,
            plantillaInterfazPokemon = content.transform.Find("PlantillaPokemonPokedex").gameObject,
            interfazPokemon;
        int idPokemonInicioGeneracion = int.Parse(limitesPokemonGeneracion.Split('/')[0]), 
            idPokemonFinGeneracion = int.Parse(limitesPokemonGeneracion.Split('/')[1]);
        UtilidadesEscena.eliminarHijosGameObject(content);
        //poner try catch
        PokemonEncontrado pokemon = null;
        for (int i = idPokemonInicioGeneracion; i <= idPokemonFinGeneracion; i++) {
            pokemon = DatosGuardarJugador.PokemonsEncontradosJugador.Find(g => g.Id == i);
            interfazPokemon = Instantiate(plantillaInterfazPokemon);
            if (pokemon != null)
            {
                interfazPokemon.GetComponentsInChildren<Image>()[3].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + i).First();
                interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[0].text = i.ToString();
                interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[1].text = pokemon.Nombre;
            }
            interfazPokemon.transform.SetParent(content.transform);
            interfazPokemon.transform.localScale = new Vector3(1, 1, 1);
            interfazPokemon.SetActive(true);
        }
        scrollViewPokemons.SetActive(true);
    }
}
