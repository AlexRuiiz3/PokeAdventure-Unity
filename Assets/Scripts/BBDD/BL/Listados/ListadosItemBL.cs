using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosItemBL
{
    public static Item obtenerItemAleatorio()
    {
        return ListadosItemDAL.obtenerItemAleatorio();
    }
    public static List<Item> obtenerItems()
    {
        return ListadosItemDAL.obtenerItems();
    }
    public static List<ItemConCantidad> obtenerItemsJugador(int id)
    {
        return ListadosItemDAL.obtenerItemsJugador(id);
    }
}
