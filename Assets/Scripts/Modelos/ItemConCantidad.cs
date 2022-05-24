/*
 * Clase: ItemConCantidad
 * 
 * Comentario: Esta clase representa a los ItemConCantidad item que tienen los jugadores y por lo tanto necesitan una cantidad. Hereda de la clase Item.
 * 
 * Atributos:
 *          Basicos:
 *              mt: Entero, Consultable, Modificable
 *          Derivados: Ninguno.
 *          Compartidos: Ninguno.
 *          
 * Metodos fundamentales(Propiedades)
 *              Cantidad: public int Cantidad{get; set;}
 * 
 * Metodos añadidos: Ninguno.
 * 
 * Metodos heredados: public override bool Equals(object obj)
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemConCantidad : Item
{
    #region Constructores 
    //Constructor con parametros
    public ItemConCantidad(Item item, int cantidad) : base(item.ID, item.Nombre, item.Descripcion, item.IndiceExito, item.CuracionPS, item.Precio,item.Tipo) {
        Cantidad = cantidad;
    }
    #endregion
    
    #region Metodos Fundalementales(Propiedades)
    //Cantidad
    public int Cantidad { get; set; }
    #endregion
    
    #region Metodos Heredados
   /* 
    * Criterio de igualdad: 
    *                       true: Si los items tienen el mismo id o son la misma instancia.
    *                       false: Si los items no tiene el mismo id o no son la misma instancia.
    */
    public override bool Equals(object obj)
    {
        bool iguales = false;

        if (this == obj)
        {
            iguales = true;
        }
        else if (obj != null && typeof(ItemConCantidad).IsInstanceOfType(obj)){
            ItemConCantidad item = (ItemConCantidad) obj;
            if (ID == item.ID)
            {
                iguales = true;
            }
        }
        return iguales;
    }
}
