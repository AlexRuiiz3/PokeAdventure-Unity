using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListadosJugadorBL
{
    public static int obtenerIDJugador(string nombreUsuario, string contrasenha)
    {
        return ListadosJugadorDAL.obtenerIDJugador(nombreUsuario,contrasenha);
    }    
}
