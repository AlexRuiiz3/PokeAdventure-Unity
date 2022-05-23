using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DialogEstate {PLAY,END}
public class ControlDialogos : MonoBehaviour //Script que ira en el gameObject que sera donde se muestre el dialogo
{
    public Queue<string> colaTextos = new Queue<string>();
    public string[] ListaFrases;
    [SerializeField] public TextMeshProUGUI textoPantalla;
    private bool reproduciendoTexto;

     /// <summary>
    /// Cabecera: public void activarDialogo()
    /// Comentario: Este metodo se encarga de preparar e iniciar un dialogo
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se preparara e iniciara un dialogo
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Cabecera: public void siguienteFrase()
    /// Comentario: Este metodo se encarga de avanzar a la siguiente frase que haya en un dialogo.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ningnua
    /// Postcondiciones: Se pasara a la siguiente frase de un dialogo. Si ya no hay mas frases y sea terminado el dialogo, se realizara una accion de fin de dialogo.
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Cabecera: IEnumerator mostrarTextoPorCaracteres(string frase)
    /// Comentario: Esta corrutina se encarga del texto que se muestra por pantalla configurarlo para que se muestre caracter a caracter.
    /// Entradas: string frase
    /// Salidas: Ninguna
    /// Precondiciones: frase debe ser distinto a null
    /// Postcondiciones: Se mostrara en pantalla una frase caracter por caracter.
    /// </summary>
    /// <param name="frase"></param>
    /// <returns></returns>
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
