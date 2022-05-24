using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosMovimientoBL
{
    /// <summary>
    /// Cabecera: public static List<MovimientoPokemon> obtenerMovimientosPokemon(int idJugador, int idPokemon, int numeroPokemon)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerMovimientosPokemon de la clase ListadosMovimientoDAL de la capa DAL.
    /// Entradas: int idJugador, int idPokemon, int numeroPokemon
    /// Salidas: List<MovimientoPokemon> movimientos
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo MovmientosPokemon, si se produce alguna excepcion o no se encuentran resultados en la consulta, 
    ///                  la lista devuelta estara vacia.
    /// </summary>
    /// <param name="idJugador"></param>
    /// <param name="idPokemon"></param>
    /// <param name="numeroPokemon"></param>
    /// <returns>List<MovimientoPokemon></returns>
    public static List<MovimientoPokemon> obtenerMovimientosPokemon(int idJugador, int idPokemon, int numeroPokemon)
    {
        return ListadosMovimientoDAL.obtenerMovimientosPokemon(idJugador, idPokemon, numeroPokemon);
    }
}
