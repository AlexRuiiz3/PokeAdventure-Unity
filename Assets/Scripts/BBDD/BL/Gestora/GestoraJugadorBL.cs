using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoraJugadorBL
{
    public static void insertarJugador(ClsJugador jugador) 
    {
        GestoraJugadorDAL.insertarJugador(jugador);
    }
}
