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
    
    /// <summary>
    /// Cabecera: public void activarMenuPokedex()
    /// Comentario: Este metodo se encarga de configurar y mostrar el menu de pokedex
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se mostrara el menu de la pokedex
    /// </summary>
    public void activarMenuPokedex() {
        GameObject menuPokedex = gameObject; //gameObject hace referencia a el gameObject que tendra el script en este caso menuPokedex
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador; 
        //Text de Menu Pokedex
        menuPokedex.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Pokemons encontrados {DatosGuardarJugador.PokemonsEncontradosJugador.Count}/898";
        menuPokedex.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Pokemons atrapados {jugador.EquipoPokemon.Count + DatosGuardarJugador.PokemonsAlmacenadosPC.Count}";

        menuPokedex.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public void configurarPrepararMenuPokemonsGeneracion(string limitesPokemonGeneracion)
    /// Comentario: Este metodo se encarga de mostrar un menu con un listado de los pokemons de una generacion, en funcion del string recibido se determinara de que generacion se trata.
    /// Entradas: string limitesPokemonGeneracion
    /// Salidas: Ninguna
    /// Precondiciones: limitesPokemonGeneracion debera estar divido por / teniendo tanto delante como detras un numero entero 
    /// Postcondiciones: Se muestra un menu con un listado de los pokemons de una generacion. La imagen que se muestra de los pokemons puede tomar dos valores:
    ///                  1: Si el jugador tiene registrado que sea encontrado un pokemon de esa generacion, se muestra una imagen y el nombre del pokemon. 
    ///                  2: Si el jugador no tiene registrado que sea encontrado con un pokemon de esa generacion, se muestra una imagen y un nombre de desconocido.  
    ///
    /// </summary>
    /// <param name="limitesPokemonGeneracion"></param>
    public void configurarPrepararMenuPokemonsGeneracion(string limitesPokemonGeneracion) {
        GameObject scrollViewPokemons = gameObject.transform.Find("ScrollViewPokemonsGeneraciones").gameObject;
        scrollViewPokemons.GetComponentInChildren<Scrollbar>().value = 1; //Esto no funciona no resetea la barra de scroll
        
        GameObject content = scrollViewPokemons.transform.GetChild(0).Find("Content").gameObject,
            plantillaInterfazPokemon = content.transform.Find("PlantillaPokemonPokedex").gameObject,
            interfazPokemon;
            
        int idPokemonInicioGeneracion = int.Parse(limitesPokemonGeneracion.Split('/')[0]), 
            idPokemonFinGeneracion = int.Parse(limitesPokemonGeneracion.Split('/')[1]);
        UtilidadesEscena.eliminarHijosGameObject(content);
        
        PokemonEncontrado pokemon = null;
        for (int i = idPokemonInicioGeneracion; i <= idPokemonFinGeneracion; i++) {
            pokemon = DatosGuardarJugador.PokemonsEncontradosJugador.Find(g => g.Id == i);
            interfazPokemon = Instantiate(plantillaInterfazPokemon);
            if (pokemon != null) //Si el pokemon no es null, es decir se encontro dentro de la lista de pokemons encontrados del jugador 
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
