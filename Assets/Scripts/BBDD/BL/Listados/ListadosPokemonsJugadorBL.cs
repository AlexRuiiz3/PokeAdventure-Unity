using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosPokemonsJugadorBL
{
    /// <summary>
    /// Cabecera: public static List<PokemonJugador> obtenerPokemonsNoEquipadosJugador(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerPokemonsNoEquipadosJugador de la clase ListadosPokemonsJugadorDAL de la capa DAL.
    /// Entradas: int id
    /// Salidas: List<PokemonJugador> 
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista con objetos de tipo PokemonJugador. Si se produce una excepcion o no se encuentra un jugador con el id 
    ///                  recibido o la consulta no tiene resultados, la lista devuelta estara vacia. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<PokemonJugador></returns>
    public static List<PokemonJugador> obtenerPokemonsNoEquipadosJugador(int id)
    {
        return ListadosPokemonsJugadorDAL.obtenerPokemonsNoEquipadosJugador(id);
    }

    /// <summary>
    /// Cabecera: public static List<PokemonJugador> obtenerPokemonsJugadorEquipados(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerPokemonsJugadorEquipados de la clase ListadosPokemonsJugadorDAL de la capa DAL.
    /// Entradas: int id
    /// Salidas: List<PokemonJugador> 
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista con objetos de tipo PokemonJugador. Si se produce una excepcion o no se encuentra un jugador con el id 
    ///                  recibido o la consulta no tiene resultados, la lista devuelta estara vacia. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<PokemonJugador></returns>
    public static List<PokemonJugador> obtenerPokemonsJugadorEquipados(int id)
    {
        return ListadosPokemonsJugadorDAL.obtenerPokemonsJugadorEquipados(id);
    }
    
    /// <summary>
    /// Cabecera: public static int obtenerNumeroPokemonsJugador(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerNumeroPokemonsJugador de la clase ListadosPokemonJugadorDAL de la capa DAL.
    /// Entradas: int id
    /// Salidas: int
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un entero con el numero de pokemons que tiene un jugador. Si se produce una excepcion o no se encuentra un jugador con el id 
    ///                  recibido o la consulta no tiene resultados, se devolvera 0. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>int</returns>
    public static int obtenerNumeroPokemonsJugador(string nombreUsuario, string contrasenha)
    {
        return ListadosPokemonsJugadorDAL.obtenerNumeroPokemonsJugador(nombreUsuario,contrasenha);
    }
}

