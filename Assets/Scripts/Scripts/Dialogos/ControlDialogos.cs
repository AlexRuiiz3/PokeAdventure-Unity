using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlDialogos : MonoBehaviour //Script que ira en el gameObejct que sera donde se muestre el dialogo
{
    public Queue <string> colaTextos = new Queue<string>();
    public string[] ListaFrases;
    [SerializeField] public TextMeshProUGUI textoPantalla;

    public void activarDialogo()
    {
        colaTextos.Clear();
        foreach (string texto in ListaFrases)
        {
            colaTextos.Enqueue(texto);//Se guarda el texto en la cola
        }
    }

    public void siguienteFrase() {
        
        if (colaTextos.Count != 0)
        {
            textoPantalla.text = "";
            StartCoroutine(mostrarTextoPorCaracteres(colaTextos.Dequeue()));
        }
        else {
            StopCoroutine(mostrarTextoPorCaracteres(""));
        }
    }

    IEnumerator mostrarTextoPorCaracteres(String frase) {
        foreach (char caracter in frase.ToCharArray()) { 
            textoPantalla.text += caracter;
            yield return new WaitForSeconds(0.04f);
        }
    }
}
