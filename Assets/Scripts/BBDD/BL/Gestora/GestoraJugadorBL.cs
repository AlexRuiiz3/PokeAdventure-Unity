using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoraJugadorBL
{
    /// <summary>
    /// Cabecera: public static void insertarJugador(ClsJugador jugador)
    /// Comentario: Este metodo se encarga de llamar al metodo insertarJugador de la clase GestoraJugadorDAL de la capa DAL.
    /// Entradas: ClsJugador jugador
    /// Salidas: Ninguna
    /// Precondiciones: jugador no debera ser distinto de null
    /// Postcondiciones: Se insertara en la base de datos un nuevo jugador. Si se produce algun tipo de excepcion el jugador no se einsertara en la base de datos.
    /// </summary>
    /// <param name="jugador"></param>
    /// <returns></returns>
    public static void insertarJugador(ClsJugador jugador) 
    {
        GestoraJugadorDAL.insertarJugador(jugador);
    }
}
