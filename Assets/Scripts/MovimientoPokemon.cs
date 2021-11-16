using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
namespace Scripts
{

    public class MovimientoPokemon
    {
        #region Atributos
        private int mt;
        private string nombre;
        private int danho;
        private int precision;
        private int pp;
        private string tipo;
        #endregion

        #region Constructor por defecto
        public MovimientoPokemon() {
            mt = 0;
            nombre = "";
            danho = 0;
            precision = 0;
            pp = 0;
            tipo = "";
        }
        #endregion

        #region Constructor con parametros
        public MovimientoPokemon(int mt, string nombre, int danho, int precision, int pp, string tipo) {
            this.mt = mt;
            this.nombre = nombre;
            this.danho = danho;
            this.precision = precision;
            this.pp = pp;
            this.tipo = tipo;
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
        public int Precision { get; }
        //pp
        public int PP { get; }
        //tipo
        public string Tipo { get; }
        #endregion
    }

}

