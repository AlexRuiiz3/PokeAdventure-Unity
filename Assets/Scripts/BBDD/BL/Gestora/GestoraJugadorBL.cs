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
    
    /// <summary>
    /// Cabecera: public static int actualizarDineroJugador(int id, int dinero)
    /// Comentario: Este metodo se encarga de llamar al metodo actualizarDineroJugador de la clase GestoraJugadorDAL de la capa DAL.
    /// Entradas: int id, int dinero
    /// Salidas: int
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se insertara en la base de datos un nuevo jugador. Si se produce algun tipo de excepcion el jugador no se einsertara en la base de datos.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dinero"></param>
    /// <returns>int</returns>
    public static int actualizarDineroJugador(int id, int dinero) {
        return GestoraJugadorDAL.actualizarDineroJugador(id,dinero);
    }
}
