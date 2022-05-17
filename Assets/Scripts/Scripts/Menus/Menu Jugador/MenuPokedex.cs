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
    public GameObject menuPokedex;

    public void configurarPrepararMenuPokemonsGeneracion(string limitesPokemonGeneracion) {
        //menuPokedex = gameObject;
        GameObject scrollViewPokemons = menuPokedex.transform.Find("ScrollViewPokemonsGeneraciones").gameObject;
        scrollViewPokemons.GetComponentInChildren<Scrollbar>().value = 1;
        
        GameObject content = scrollViewPokemons.transform.GetChild(0).Find("Content").gameObject,
            plantillaInterfazPokemon = content.transform.Find("PlantillaPokemonPokedex").gameObject,
            interfazPokemon;
        int idPokemonInicioGeneracion = int.Parse(limitesPokemonGeneracion.Split('/')[0]), 
            idPokemonFinGeneracion = int.Parse(limitesPokemonGeneracion.Split('/')[1]);
        UtilidadesEscena.eliminarHijosGameObject(content);
        //Cambiar nombre usuario y poner try catch
        List<PokemonEncontrado> pokemonsEncontrados = ListadosPokemonEncontradosJugadorBL.obtenerPokemonsEncontradosDeJugador("a");
        PokemonEncontrado pokemon = null;
        for (int i = idPokemonInicioGeneracion; i <= idPokemonFinGeneracion; i++) {
            pokemon = pokemonsEncontrados.Find(g => g.Id == i);
            interfazPokemon = Instantiate(plantillaInterfazPokemon);
            if (pokemon != null)
            {
                interfazPokemon.GetComponentsInChildren<Image>()[3].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + i).First();
                interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[0].text = i.ToString();
                interfazPokemon.GetComponentsInChildren<TextMeshProUGUI>()[1].text = pokemon.Nombre;

                interfazPokemon.transform.SetParent(content.transform);
                interfazPokemon.transform.localScale = new Vector3(1, 1, 1);
                interfazPokemon.SetActive(true);
            }
            else {
                interfazPokemon.transform.SetParent(content.transform);
                interfazPokemon.transform.localScale = new Vector3(1,1,1);
                interfazPokemon.SetActive(true);
            }
        }
        scrollViewPokemons.SetActive(true);
    }
}
