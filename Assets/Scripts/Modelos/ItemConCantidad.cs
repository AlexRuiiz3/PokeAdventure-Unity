using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemConCantidad : Item
{
    public ItemConCantidad(Item item, int cantidad) : base(item.ID, item.Nombre, item.Descripcion, item.IndiceExito, item.CuracionPS, item.Tipo) {
        Cantidad = cantidad;
    }

    public int Cantidad { get; set; }

}

