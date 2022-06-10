using PokeAPI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class APIListadosPokemonBL 
{

    /// <summary>
    /// Cabecera: public static async Task<PokeAPI.Pokemon> obtenerPokemonDeApi(int id)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerPokemonDeApi de la clase APIListadosPokemonDAL de la capa DAL.
    /// Entradas: int id
    /// Salidas: PokeAPI.Pokemon  
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo PokeAPI.Pokemon. Si el id no coincide con el de ningun pokemon, se devolvera null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>PokeAPI.Pokemon</returns>
    public static async Task<PokeAPI.Pokemon> obtenerPokemonDeApi(int id)
    {
        return await APIListadosPokemonDAL.obtenerPokemonDeApi(id);
    }
    
    /// <summary>
    /// Cabecera: public static async Task<List<string>> obtenerNombreTiposPokemon(PokemonTypeMap[] tiposPokemon)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerNombreTiposPokemon de la clase APIListadosPokemonDAL de la capa DAL.
    /// Entradas: PokemonTypeMap[] tiposPokemon
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de string que tendra como valores los tipos de un pokemon. Si se produce algun error, se devolvera la lista vacia.
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <returns>List<string></returns>
    public static async Task<List<string>> obtenerNombreTiposPokemon(PokemonTypeMap[] tiposPokemon)
    {
        return await APIListadosPokemonDAL.obtenerNombreTiposPokemon(tiposPokemon);
    }
    
    /// <summary>
    /// Cabecera: public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(PokemonTypeMap[] tiposPokemon)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerNombreTiposDebilesPokemon de la clase APIListadosPokemonDAL de la capa DAL.
    /// Entradas: PokemonTypeMap[] tiposPokemon
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de string que tendra como valores los tipos de un pokemon. Si se produce algun error, se devolvera la lista vacia.
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <returns>List<string></returns>
    public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(PokemonTypeMap[] tiposPokemon)
    {
        return await APIListadosPokemonDAL.obtenerNombreTiposDebilesPokemon(tiposPokemon);
    }

    /// <summary>
    /// Cabecera: public static async Task<List<MovimientoPokemon>> obtenerMovimientosAleatoriosPokemon(PokemonMove[] pokemonMoves)
    /// Comentario: Este metodo se encarga de llamar al metodo obtenerMovimientosAleatoriosPokemon de la clase APIListadosPokemonDAL de la capa DAL.
    /// Entradas: PokemonMove[] pokemonMoves
    /// Salidas: Task<List<MovimientoPokemon>>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera con los movimientos pokemon mappeados a MovimientoPokemon
    /// </summary>
    /// <param name="pokemonMoves"></param>
    /// <returns>Task<List<MovimientoPokemon>></returns>
    public static async Task<List<MovimientoPokemon>> obtenerMovimientosAleatoriosPokemon(PokemonMove[] pokemonMoves)
    {
        return await APIListadosPokemonDAL.obtenerMovimientosAleatoriosPokemon(pokemonMoves);
    }
}
