using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokeApiNet;
using System.Net;

public class Utilidades
{
    public static bool comprobarCadenaVacia(string cadena)
    {
        return string.IsNullOrEmpty(cadena.Trim());
    }

    public static byte[] obtenerGifUrl()
    {
        using (WebClient webClient = new WebClient())
        {
            byte[] data = webClient.DownloadData("https://fbcdn-sphotos-h-a.akamaihd.net/hphotos-ak-xpf1/v/t34.0-12/10555140_10201501435212873_1318258071_n.jpg?oh=97ebc03895b7acee9aebbde7d6b002bf&oe=53C9ABB0&__gda__=1405685729_110e04e71d9");

        }
    }
}
