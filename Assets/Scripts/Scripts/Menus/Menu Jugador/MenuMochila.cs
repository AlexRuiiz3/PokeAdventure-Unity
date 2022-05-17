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
    public GameObject menuMochila;
    private Jugador jugador;
    GameObject interfazItemUsar;

    public void prepararMenuMochila(GameObject plantillaItem) {
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        GameObject contentListMochila = plantillaItem.transform.parent.gameObject;
        GameObject interfazItem;
        UtilidadesEscena.eliminarHijosGameObject(contentListMochila); //Necesario sino salen duplicados los items
        jugador.EquipoPokemon[0].HPMaximos = 100;
        jugador.EquipoPokemon[0].HP = 50;
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
        menuMochila.SetActive(true);
    }

    public void clickButtonUsarItem(GameObject interfazItem) {
        interfazItemUsar = interfazItem;
        GameObject menuAplicarItem = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuAplicarItemPokemon");
        PokemonJugador pokemon;
        GameObject gridInterfazPokemons = menuAplicarItem.transform.Find("Pokemons").gameObject,
            interfazPokemon;
        bool pokemonsFullVida = true;

        var mensajeNoHayPokemons = menuAplicarItem.transform.Find("TextMensaje"); //Text que informa cuando no hay pokemons que necesiten curaciones
        mensajeNoHayPokemons.gameObject.SetActive(false);
         
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
                interfazPokemon.GetComponentsInChildren<Image>()[2].transform.localScale = new Vector3((float)pokemon.HP / pokemon.HPMaximos, 1f, 1f);
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

    public void aplicarItemAPokemon(GameObject interfazPokemon){
        ItemConCantidad itemAplicar = jugador.Mochila.Find(g => g.ID == Int16.Parse(interfazItemUsar.name));
        
        if (--itemAplicar.Cantidad == 0)
        {
            Destroy(interfazItemUsar);
            jugador.Mochila.Remove(itemAplicar);
        }
        else {
            interfazItemUsar.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"x{itemAplicar.Cantidad}";
            jugador.EquipoPokemon.Find(g => g.ID == Int16.Parse(interfazPokemon.name)).HP += itemAplicar.CuracionPS;
        }
        Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuAplicarItemPokemon").SetActive(false);
    }


}
