using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionPerfil : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void cambiarFoto(GameObject buttonImage) {
        Debug.Log("Cambiar foto");
    }
}
