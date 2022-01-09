using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilidades
{
    public static bool comprobarCadenaVacia(string cadena) {
        return string.IsNullOrEmpty(cadena.Trim());
    }
}
