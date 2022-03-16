using PokeAPI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class APIListadosPokemonBL 
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static async Task<PokeAPI.Pokemon> obtenerPokemonDeApi(int id)
    {
        return await APIListadosPokemonDAL.obtenerPokemonDeApi(id);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    public static async Task<List<string>> obtenerNombreTiposPokemon(PokemonTypeMap[] tiposPokemon, string idioma)
    {
        return await APIListadosPokemonDAL.obtenerNombreTiposPokemon(tiposPokemon,idioma);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(PokemonTypeMap[] tiposPokemon, string idioma)
    {
        return await APIListadosPokemonDAL.obtenerNombreTiposDebilesPokemon(tiposPokemon,idioma);
    }
}
