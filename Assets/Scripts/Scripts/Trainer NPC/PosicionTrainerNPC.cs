using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionTrainerNPC
{
    public PosicionTrainerNPC(Vector2 posicion, List<string> orientaciones) {
        Posicion = posicion;
        Orientaciones = orientaciones;
    }
    public Vector2 Posicion { get; }
    public List<string> Orientaciones { get; }
}
