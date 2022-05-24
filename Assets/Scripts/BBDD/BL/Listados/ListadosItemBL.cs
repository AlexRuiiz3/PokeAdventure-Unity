using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosItemBL
{
    /// <summary>
    /// Cabecera: public static Item obtenerItemAleatorio()
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerItemAleatorio de la clase ListadosItemDAL de la capa DAL. 
    /// Entradas: Ninguna
    /// Salidas: Item item
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo Item de la base de datos de forma aleatoria.
    ///                  Si se produce alguna excepcion se devolvera un objeto de tipo Item con los valores por defecto.
    /// </summary>
    /// <returns>Item</returns>
    public static Item obtenerItemAleatorio()
    {
        return ListadosItemDAL.obtenerItemAleatorio();
    }
    
    /// <summary>
    /// Cabecera: public static List<Item> obtenerItems()
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerItems de la clase ListadosItemDAL de la capa DAL.
    /// Entradas: Ninguna
    /// Salidas: List<Item> item
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo Item que existen en la base de datos.
    ///                  Si se produce alguna excepcion se devolvera una lista de Item vacia.
    /// </summary>
    /// <returns>List<Item></returns>
    public static List<Item> obtenerItems()
    {
        return ListadosItemDAL.obtenerItems();
    }
    
    /// <summary>
    /// Cabecera: public static List<ItemConCantidad> obtenerItemsJugador(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerItemsJugador de la clase ListadosItemDAL de la capa DAL.
    /// Entradas: int id
    /// Salidas: List<ItemConCantidad> items
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo ItemConCantidad que tiene un jugador especifico.
    ///                  Si se produce alguna excepcion o el id recibido no coincide con el de ningun jugador o el jugador no tiene ningun item asignado 
    ///                  se devolvera una lista de Item vacia.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<ItemConCantidad></returns>
    public static List<ItemConCantidad> obtenerItemsJugador(int id)
    {
        return ListadosItemDAL.obtenerItemsJugador(id);
    }
    
    /// <summary>
    /// Cabecera: public static Item obtenerItem(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerItem de la clase ListadosItemDAL de la capa DAL. 
    /// Entradas: int id
    /// Salidas: Item item
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo ItemConCantidad que tiene un jugador especifico.
    ///                  Si se produce alguna excepcion o no se encuentra ningun item con el id recibido, se devolvera un item con los valores por defecto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Item</returns>
    public static Item obtenerItem(int id)
    {
        return ListadosItemDAL.obtenerItem(id);
    }
}
