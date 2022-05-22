using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DialogEstate { PLAY,END}
public class ControlDialogos : MonoBehaviour //Script que ira en el gameObejct que sera donde se muestre el dialogo
{
    public Queue<string> colaTextos = new Queue<string>();
    public string[] ListaFrases;
    [SerializeField] public TextMeshProUGUI textoPantalla;
    private bool reproduciendoTexto;

    public void activarDialogo()
    {
        GameObject jugador = GameObject.Find("Player");

        //Se pausa el movimiento y animacion de jugador
        if (SceneManager.GetActiveScene().name != "GetFirstPokemonScene") {
            jugador.GetComponent<PlayerController>().setMovimientoHorizontal(0);
            jugador.GetComponent<PlayerController>().setMovimientoVertical(0);
            jugador.GetComponent<Animator>().SetBool("isMoving", false);
        }

        colaTextos.Clear();
        foreach (string texto in ListaFrases)
        {
            colaTextos.Enqueue(texto);//Se guarda el texto en la cola
        }
        siguienteFrase();
    }

    public void siguienteFrase()
    {
        if (colaTextos.Count != 0 && !reproduciendoTexto)
        {
            PlayerPrefs.SetString("EstadoDialogo", DialogEstate.PLAY.ToString());
            textoPantalla.text = "";
            StartCoroutine(mostrarTextoPorCaracteres(colaTextos.Dequeue()));
        }
        else if (colaTextos.Count == 0 && !reproduciendoTexto)
        {
            gameObject.SetActive(false);
            new UtilidadesObjetosInteractables().determinarAccionFinDialogo();
            PlayerPrefs.SetString("EstadoDialogo", DialogEstate.END.ToString());
            StopCoroutine(mostrarTextoPorCaracteres(""));
        }
    }

    IEnumerator mostrarTextoPorCaracteres(string frase)
    {
        foreach (char caracter in frase.ToCharArray())
        {
            reproduciendoTexto = true;
            textoPantalla.text += caracter;
            yield return new WaitForSeconds(0.025f);
        }
        reproduciendoTexto = false;
    }
}
