using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Clase que sirve para englobar la posicion y las orientaciones que tendra un Entrenador NPC
*/
public class PosicionTrainerNPC
{
    public PosicionTrainerNPC(Vector2 posicion, List<string> orientaciones) {
        Posicion = posicion;
        Orientaciones = orientaciones;
    }
    public Vector2 Posicion { get; }
    public List<string> Orientaciones { get; }
}
