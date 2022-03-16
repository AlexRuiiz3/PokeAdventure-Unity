using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoInteractable : MonoBehaviour
{
    public Textos textos;
    private bool Repetir { get; set; } = true;

    void Update()
    {
       /* if (Input.GetKey(KeyCode.Escape) && Repetir)
        {
            FindObjectOfType<ControlDialogos>().activarDialogo(textos);
            Repetir = false;
        }*/
    }
}
