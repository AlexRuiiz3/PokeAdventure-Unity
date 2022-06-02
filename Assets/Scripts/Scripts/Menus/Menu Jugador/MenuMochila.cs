using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuMochila : MonoBehaviour
{
    public GameObject interfazItemUsar;
    private Jugador jugador;

    /// <summary>
    /// Cabecera: public void prepararMenuMochila(GameObject plantillaItem)
    /// Comentario: Este metodo de configuarar y mostrar el menu mochila del jugador, con los items que este tenga.
    /// Entradas: GameObject plantillaItem
    /// Salidas: Ninguna
    /// Precondiciones: plantillaItem no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se muestra el menu de la mochila del jugador el cual contendra todos los items que tiene el jugador.
    /// </summary>
    /// <param name="plantillaItem"></param>
    public void prepararMenuMochila(GameObject plantillaItem) {
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        GameObject contentListMochila = plantillaItem.transform.parent.gameObject;
        GameObject interfazItem;
        UtilidadesEscena.eliminarHijosGameObject(contentListMochila); //Necesario sino salen duplicados los items
        foreach (ItemConCantidad item in jugador.Mochila) {
            interfazItem = Instantiate(plantillaItem);

            interfazItem.name = item.ID.ToString();
            interfazItem.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>($"Imagenes/Items/{item.Nombre}");
            interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"x{item.Cantidad}";

            if (item.Tipo == "Pocion") {
                interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = $"{item.Nombre}.{item.CuracionPS}PS";
            }
            else {
                interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = item.Nombre;
                Destroy(interfazItem.transform.Find("Button").gameObject);
            }
            interfazItem.transform.SetParent(contentListMochila.transform);
            interfazItem.transform.localScale = plantillaItem.transform.localScale;
            interfazItem.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public void clickButtonUsarItem(GameObject interfazItem)
    /// Comentario: Este metodo se encarga de configurar y mostrar un menu que contiene los pokemons a los que se les puede aplicar un item.
    /// Entradas: GameObject interfazItem
    /// Salidas: Ninguna
    /// Precondiciones: interfazItem no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se muestra el menu con los pokemons a los que se le puede aplicar un item
    /// </summary>
    /// <param name="interfazItem"></param>
    public void clickButtonUsarItem(GameObject interfazItem) {
        interfazItemUsar = interfazItem;
        GameObject menuAplicarItem = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuAplicarItemPokemon");
        PokemonJugador pokemon;
        GameObject gridInterfazPokemons = menuAplicarItem.transform.Find("Pokemons").gameObject,
            interfazPokemon;
        bool pokemonsFullVida = true;

        var mensajeNoHayPokemons = menuAplicarItem.transform.Find("TextMensaje"); //Text que informa cuando no hay pokemons que necesiten curaciones
        mensajeNoHayPokemons.gameObject.SetActive(false);
        
        //Por cada pokemon que haya en el equipo del jugador se comprueba su vida y se obtiene su interfaz
        for (int i = 0; i < jugador.EquipoPokemon.Count; i++) {
            pokemon = jugador.EquipoPokemon[i];
            interfazPokemon = gridInterfazPokemons.transform.GetChild(i).gameObject;
            if (pokemon.HP < pokemon.HPMaximos)
            {
                pokemonsFullVida = false;
                interfazPokemon = gridInterfazPokemons.transform.GetChild(i).gameObject;
                interfazPokemon.name = pokemon.ID.ToString();
                interfazPokemon.GetComponentsInChildren<Text>()[0].text = pokemon.Nombre;
                interfazPokemon.GetComponentsInChildren<Text>()[1].text = $"lv. {pokemon.Nivel}";
                interfazPokemon.GetComponentsInChildren<Text>()[2].text = $"PS: {pokemon.HP} / {pokemon.HPMaximos}";
                UtilidadesSystemaBatalla.modificarBarraSalud(interfazPokemon.GetComponentsInChildren<Image>()[2], pokemon.HP, pokemon.HPMaximos);
                interfazPokemon.GetComponentsInChildren<Image>()[4].sprite = Resources.LoadAll<Sprite>("Imagenes/Pokemons/Front/" + pokemon.ID).First();
                interfazPokemon.SetActive(true);
            }
            else {
                interfazPokemon.SetActive(false); //Se pone a false, porque sino un pokemon que haya sido curado la siguiente vez se saldra, por lo tanto los que no necesiten curacion se desactivan
            }
        }
        if (pokemonsFullVida) {
            mensajeNoHayPokemons.gameObject.SetActive(true);
        }
        menuAplicarItem.SetActive(true);
    }

    /// <summary>
    /// Cabecera: public void aplicarItemAPokemon(GameObject interfazPokemon)
    /// Comentario: Este metodo se encarga de aplicar un item a un pokemon, recuperando parte de la vida.
    /// Entradas: GameObject interfazPokemon
    /// Salidas: Ninguna
    /// Precondiciones: interfazPokemon no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se cura la vida del pokemon que se selecciono, se resta uno a la cantidad del item usado y si la cantidad del item pasa a ser 0,
    ///                  se elimina de la la mochila del jugador y se destruye su interfaz.
    /// </summary>
    /// <param name="interfazPokemon"></param>
    public void aplicarItemAPokemon(GameObject interfazPokemon){
        ItemConCantidad itemAplicar = jugador.Mochila.Find(g => g.ID == Int16.Parse(interfazItemUsar.name));
        --itemAplicar.Cantidad;
        UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/UsarPocion",1f);
        jugador.EquipoPokemon.Find(g => g.ID == Int16.Parse(interfazPokemon.name)).HP += itemAplicar.CuracionPS;
        if (itemAplicar.Cantidad == 0)
        {
            Destroy(interfazItemUsar);
            jugador.Mochila.Remove(itemAplicar);
        }else{
            interfazItemUsar.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"x{itemAplicar.Cantidad}";
        }
        Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuAplicarItemPokemon").SetActive(false);
    }
}
