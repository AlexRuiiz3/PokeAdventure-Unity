using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Textos //Clase para guardar de una forma mas comoda los mensajes
{
    [TextArea (1,7)]//Inicar el minimo y el maximo que puede existir
    public string[] ListadoTextos;
}
