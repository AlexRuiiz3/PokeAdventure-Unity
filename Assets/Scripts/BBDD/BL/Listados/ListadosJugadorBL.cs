using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListadosJugadorBL
{
    /*
    public static int obtenerIDJugador(string nombreUsuario, string contrasenha)
    {
        return ListadosJugadorDAL.obtenerIDJugador(nombreUsuario,contrasenha);
    }    */

    public static bool comprobarExistenciaNombreUsuarioContrasenha(string nombreUsuario, string contrasenha) {
        return ListadosJugadorDAL.comprobarExistenciaNombreUsuarioContrasenha(nombreUsuario,contrasenha);
    }

    public static ClsJugador obtenerJugador(string nombreUsuario, string contrasenha)
    {
        return ListadosJugadorDAL.obtenerJugador(nombreUsuario, contrasenha);
    }
}
