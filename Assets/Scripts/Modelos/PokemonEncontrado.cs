/*
 * Clase: PokemonEncontrado
 * 
 * Comentario: Esta clase representa los datos de un pokemon que son necesarion para identificar a un pokemon cuando el jugador se encuentre con este.
 * 
 * Atributos:
 *          Basicos:
 *              id: Entero, Consultable, Modificable.
 *              nombre: Cadena, Consultable, Modificable.
 *          Derivados: Ninguno.
 *          Compartidos: Ninguno.
 *          
 * Metodos fundamentales(Propiedades)
 *              ID: public int ID { get; }
 *              Nombre: public string Nombre { get; set; }
 *
 * Metodos añadidos: Ninguno.
 * 
 * Metodos heredados: Ninguno.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PokemonEncontrado
{
    #region Constructores
    //Constructor con parametros
    public PokemonEncontrado(int id, string nombre) {
        Id = id;
        Nombre = nombre;
    }
    #endregion
        
    #region Metodos Fundamentales(Propiedades)
    public int Id { get; }
    public string Nombre { get; set; }
    #endregion 
}

