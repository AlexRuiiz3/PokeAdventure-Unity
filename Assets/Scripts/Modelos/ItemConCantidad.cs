using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemConCantidad : Item
{
    public ItemConCantidad(Item item, int cantidad) : base(item.ID, item.Nombre, item.Descripcion, item.IndiceExito, item.CuracionPS, item.Precio,item.Tipo) {
        Cantidad = cantidad;
    }

    public int Cantidad { get; set; }


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

