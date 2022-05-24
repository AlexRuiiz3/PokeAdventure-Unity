using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosTipoBL
{
    /// <summary>
    /// Cabecera: public static List<string> obtenerTiposPokemon(int idPokemon)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerTiposPokemon de la clase ListadosTipoDAL de la capa DAL.
    /// Entradas: int idPokemon
    /// Salidas: List<string> 
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un lista de string que contendra los tipos de un pokemon en especifico. Si se produce una excepcion o no se encuentra un pokemon 
    //                   con el id recibido o la consulta no tiene resultados, la lista devuelta estara vacia.
    /// </summary>
    /// <param name="idPokemon"></param>
    /// <returns>List<string></returns>
    public static List<string> obtenerTiposPokemon(int idPokemon)
    {
        return ListadosTipoDAL.obtenerTiposPokemon(idPokemon);
    }

    /// <summary>
    /// Cabecera: public static List<string> obtenerTiposDebilesTipo(string tipo)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerTiposDebilesTipo de la clase ListadosTipoDAL de la capa DAL.
    /// Entradas: string tipo
    /// Salidas: List<string> tiposDebiles
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un lista de string que contendra los tipos debiles de un pokemon en especifico. Si se produce una excepcion o no se encuentra un pokemon 
    //                   con el id recibido o la consulta no tiene resultados, la lista devuelta estara vacia.
    /// </summary>
    /// <param name="tipo"></param>
    /// <returns>List<string></returns>
    public static List<string> obtenerTiposDebilesTipo(string tipo)
    {
        return ListadosTipoDAL.obtenerTiposDebilesTipo(tipo);
    }
}
