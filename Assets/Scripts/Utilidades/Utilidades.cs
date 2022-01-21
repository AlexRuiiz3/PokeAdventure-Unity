using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class Utilidades
{
    public static bool comprobarCadenaVacia(string cadena)
    {
        return string.IsNullOrEmpty(cadena.Trim());
    }

    public static byte[] obtenerGifUrl(string url)
    {
        byte[] foto;
        using (WebClient webClient = new WebClient())
        {
            foto = webClient.DownloadData(url);
        }
        return foto;
    }
}
