using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListadosJugadorBL
{
    /// <summary>
    /// Cabecera: public static bool comprobarExistenciaNombreUsuarioContrasenha(string nombreUsuario, string contrasenha)
    /// Comentario: Este metodo se encarga de llamar al metodo comprobarExistenciaNombreUsuarioContrasenha de la clase ListadosJugadorDAL de la capa DAL.
    /// Entradas: string nombreUsuario, string contrasenha
    /// Salidas: bool existe
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un dato booleano cuyo valor puede ser:
    ///                  true: Cuando en la base de datos exista algun jugador cuyo nombre de usuario y contraseña sean iguales a los recibidos.
    ///                  false: Cuando en la base de datos no exista ningun jugador cuyo nombre de usuario y contrasela sean iguales a los recibidos o cuando se produzca cualquier 
    ///                         tipo de excepcion.
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <param name="contrasenha"></param>
    /// <returns>bool</returns>
    public static bool comprobarExistenciaNombreUsuarioContrasenha(string nombreUsuario, string contrasenha) {
        return ListadosJugadorDAL.comprobarExistenciaNombreUsuarioContrasenha(nombreUsuario,contrasenha);
    }

    /// <summary>
    /// Cabecera: public static ClsJugador obtenerJugador(string nombreUsuario, string contrasenha)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerJugador de la clase ListadosJugadorDAL de la capa DAL.
    /// Entradas: string nombreUsuario, string contrasenha
    /// Salidas: ClsJugador jugador
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo Jugador. Si se produce alguna excepcion o no se encuentra ningun jugador cuyo nombre de usuario 
    //                   y contraseña sean iguales a los recibidos, se devolvera null. 
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <param name="contrasenha"></param>
    /// <returns>ClsJugador</returns>
    public static ClsJugador obtenerJugador(string nombreUsuario, string contrasenha)
    {
        return ListadosJugadorDAL.obtenerJugador(nombreUsuario, contrasenha);
    }
}
