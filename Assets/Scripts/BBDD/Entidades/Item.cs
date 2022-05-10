using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    #region Constructores
    //Constructor sin parametros
    public Item() {
        ID = 0;
        Nombre = "";
        Descripcion = "";
        IndiceExito = 0;
        CuracionPS = 0;
        Tipo = "";
    }
    //Constructor con parametros
    public Item(int id, string nombre, string descripcion, int indiceExito, int curacionPS,int precio, string tipo) {
        ID = id;
        Nombre = nombre;
        Descripcion = descripcion;
        IndiceExito = indiceExito;
        CuracionPS = curacionPS;
        Precio = precio;
        Tipo = tipo;
    }
    #endregion

    #region Metodos fundamentales(Propiedades)
    public int ID { get; }
    public string Nombre { get; }
    public string Descripcion { get; }
    public int IndiceExito { get; }
    public int Precio { get; }
    public int CuracionPS { get; }
    public string Tipo { get; }
    #endregion
}


