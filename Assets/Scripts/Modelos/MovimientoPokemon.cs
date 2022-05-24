/*
 * Clase: MovimientoPokemon
 * 
 * Comentario: Esta clase representa a los movimientos que tienen los pokemon.
 * 
 * Atributos:
 *          Basicos:
 *              mt: Entero, Consultable
 *              nombre: Cadena, Consultable
 *              danho: Entero, Consultable
 *              precision: Entero, Consultable
 *              pp: Entero, Consultable
 *              tipo: Cadena, Consultable
 *          Derivados: Ninguno.
 *          Compartidos: Ninguno.
 *          
 * Metodos fundamentales(Propiedades)
 *              mt: public int MT{get}
 *              nombre: public string Nombre{get}
 *              danho: public int Danho{get}
 *              precision: public int Precision{get}
 *              pp: public int PP{get}
 *              tipo: public string Tipo{get}
 * 
 * Metodos añadidos: Ninguno.
 * 
 * Metodos heredados: Ninguno.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPokemon
{

    #region Constructor por defecto
    public MovimientoPokemon()
    {
        MT = 0;
        Nombre = "";
        Danho = 0;
        Precicion = 0;
        PP = 0;
        PPMaximo = 0;
        Tipo = "";
    }
    #endregion

    #region Constructor con parametros
    public MovimientoPokemon(int mt, string nombre, int danho, int precision, int pp, string tipo)
    {
        MT = mt;
        Nombre = nombre;
        Danho = danho;
        Precicion = precision;
        PP = pp;
        PPMaximo = pp;
        Tipo = tipo;
    }
    #endregion

    #region Metodos Fundamentales(Propiedades)
    //mt
    public int MT { get; }
    //nombre
    public string Nombre { get; }
    //danho
    public int Danho { get; }
    //precision
    public int Precicion { get; }
    //pp
    public int PP { get; }
    public int PPMaximo { get; }
    //tipo
    public string Tipo { get; }
    #endregion
}



