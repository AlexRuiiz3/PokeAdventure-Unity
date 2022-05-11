using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuTienda : MonoBehaviour
{
    public TextMeshProUGUI textSaldo;
    public GameObject plantillaIterfazItem;
    public GameObject contentListaItems;

    private int idItemComprar;
    private Jugador jugador;
    private void Start()
    {
        prepararMenuTienda();
        jugador = GameObject.Find("Player").GetComponent<PlayerController>().Jugador;
        textSaldo.text = $"Saldo {jugador.Dinero}$";
    }
    
    /// <summary>
    /// Cabecera: public void comprarItem(GameObject interfazItem)
    /// Comentario: Este metodo se encarga de determinar si un item se puede comprar y desplegar un menu de confirmacion de compra.
    /// Entradas: GameObject interfazItem
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se determinara si un item se puede comprar. Si se puede comprar se preparara un menu de confirmacion de compra, 
    ///                  si no se puede realizar la compra por cualquier motivo, se informara de ello en un menu.
    /// </summary>
    ///<param name="interfazItem"></param>
    public void comprarItem(GameObject interfazItem) {
        int precio = Int16.Parse(interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[2].text.Replace('$',' '));
        string textCantidad = interfazItem.GetComponentInChildren<InputField>().text;
        if (!string.IsNullOrEmpty(textCantidad.Trim()))
        {
            int cantidad = Int16.Parse(textCantidad);
            if (cantidad > 0)
            {
                if (precio * cantidad <= jugador.Dinero)
                {
                    idItemComprar = Int16.Parse(interfazItem.name);
                    GameObject menuConfirmacionCompra = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "MenuConfirmacion");
                    menuConfirmacionCompra.GetComponentsInChildren<Image>()[2].sprite = interfazItem.GetComponentsInChildren<Image>()[1].sprite;
                    menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Nombre: {interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text}";
                    menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"Cantidad: {cantidad}";
                    menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"{interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[2].text}";
                    menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[4].text = $"Precio final: {precio * cantidad}$";
                    menuConfirmacionCompra.SetActive(true);
                }
                else {
                    UtilidadesEscena.mostrarMensajeError($"Saldo insuficiente. Precio de compra: {precio * cantidad}$");
                }
            }
            else {
                UtilidadesEscena.mostrarMensajeError("La cantidad debe ser un numero superior a 0");
            }
        }
        else {
            UtilidadesEscena.mostrarMensajeError("La cantidad no puede estar vacia");
        }
    }
    /// <summary>
    /// Cabecera: public void confirmarCompra(GameObject menuConfirmacionCompra)
    /// Comentario: Este metodo se encarga de añadir a la mochila(Lista de items) del jugador el item a comprar y restar al dinero del jugador la cantidad total de la compra. 
    /// Entradas: GameObject menuConfirmacionCompra
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran dos acciones en funcion de si el jugador tiene ya el item a comprar en su inventario: 
    ///                  1: Si ya tiene el item, se le sumara a ese item la cantidad que el jugador haya comprado.
    ///                  2: Si no tiene el item, se añadira el item a su inventario.
    /// </summary>
    ///<param name="menuConfirmacionCompra"></param>
    public void confirmarCompra(GameObject menuConfirmacionCompra) {
        string nombreItem = menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[1].text.Split(':')[1].Trim();
        int cantidad = Int16.Parse(menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[2].text.Split(':')[1]),
            costeCompra = Int16.Parse(menuConfirmacionCompra.GetComponentsInChildren<TextMeshProUGUI>()[4].text.Split(':')[1].Replace('$',' '));
        ItemConCantidad itemJugador = jugador.Mochila.Find(g => g.Nombre == nombreItem);
        if (itemJugador != null)
        {
            itemJugador.Cantidad += cantidad;
        }
        else {
            Item itemNuevo = ListadosItemBL.obtenerItem(idItemComprar);
            jugador.Mochila.Add(new ItemConCantidad(itemNuevo,cantidad));
        }
        jugador.Dinero -= costeCompra;
        textSaldo.text = $"Saldo {jugador.Dinero}$";
        menuConfirmacionCompra.SetActive(false);
    }
    //Metodo que se encarga de configurar y preparar la interfaz del menu de la tienda 
    private void prepararMenuTienda() {
        try {
            List<Item> itemsDisponibles = ListadosItemBL.obtenerItems();
            GameObject interfazItem;
            foreach (Item item in itemsDisponibles) {

                interfazItem = Instantiate(plantillaIterfazItem);
                Image[] a = interfazItem.GetComponentsInChildren<Image>();
                interfazItem.name = item.ID.ToString();
                interfazItem.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>($"Imagenes/Items/{item.Nombre}");
                interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = item.Nombre;
                if (item.Tipo == "Pocion") {
                    interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"{item.CuracionPS}PS";
                }
                interfazItem.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"{item.Precio}$";

                interfazItem.transform.SetParent(contentListaItems.transform);
                interfazItem.transform.localScale = new Vector3(1, 1, 1);
                interfazItem.SetActive(true);
            }
        }catch(Exception){
            UtilidadesEscena.mostrarMensajeError("Error en la obteccion de los items almacenados en la base de datos");
        }        
    }
}

