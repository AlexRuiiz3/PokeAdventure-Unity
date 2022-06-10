using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosPokemonEncontradosJugadorBL
{
    /// <summary>
    /// Cabecera: public static List<PokemonEncontrado> obtenerPokemonsEncontradosDeJugador(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerPokemonsEncontradosDeJugador de la clase ListadosPokemonEncontradosJugadorDAL de la capa DAL.
    /// Entradas: in id
    /// Salidas: List<PokemonEncontrado> 
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos PokemonEncontrado. Si se produce una excepcion, 
    ///                  el id no corresponde con el de ningun jugador o la consulta no tiene resultado, la lista devuelta estara vacia.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<PokemonEncontrado></returns>
    public static List<PokemonEncontrado> obtenerPokemonsEncontradosDeJugador(int id)
    {
        return ListadosPokemonEncontradosJugadorDAL.obtenerPokemonsEncontradosDeJugador(id);
    }
}
