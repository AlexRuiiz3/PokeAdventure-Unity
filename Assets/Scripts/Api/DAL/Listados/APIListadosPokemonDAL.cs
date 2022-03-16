using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using PokeAPI;

public class APIListadosPokemonDAL
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static async Task<PokeAPI.Pokemon> obtenerPokemonDeApi(int id)
    {
        PokeAPI.Pokemon pokemonSolicitado = await DataFetcher.GetApiObject<PokeAPI.Pokemon>(id);
        return pokemonSolicitado;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    public static async Task<List<string>> obtenerNombreTiposPokemon(PokemonTypeMap[] tiposPokemon, string idioma)
    {
        List<string> nombreTiposPokemon = new List<string>();
        PokemonType tipo;
        string tipoIdioma;

        foreach (PokemonTypeMap tipoPokemon in tiposPokemon)
        {
            tipo = await DataFetcher.GetApiObject<PokemonType>(tipoPokemon.Type.ID);
            tipoIdioma = obtenerNombreTipoPokemonEnUnIdioma(tipo,idioma);
            nombreTiposPokemon.Add(tipoIdioma);
        }
        return nombreTiposPokemon;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(PokemonTypeMap[] tiposPokemon, string idioma)
    {
        List<string> listadosTiposDobleDanho = new List<string>();
        PokemonType tipo;
        PokemonType tipoDanho;
        string tipoIdioma;

        foreach (PokemonTypeMap tipoPokemon in tiposPokemon)
        {
            tipo = await DataFetcher.GetApiObject<PokemonType>(tipoPokemon.Type.ID);
            foreach (NamedApiResource<PokemonType> tipoDobleDanho in tipo.DamageRelations.DoubleDamageFrom) {
                tipoDanho = await DataFetcher.GetApiObject<PokemonType>(tipoDobleDanho.ID);
                tipoIdioma = obtenerNombreTipoPokemonEnUnIdioma(tipoDanho, idioma);
                listadosTiposDobleDanho.Add(tipoIdioma);
            }
            
        }
        return listadosTiposDobleDanho;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    private static string obtenerNombreTipoPokemonEnUnIdioma(PokemonType tipoPokemon, string idioma)
    {
        ResourceName[] listadoNombres = tipoPokemon.Names;
        string nombreTipo = (from tipo in listadoNombres
                             where tipo.Language.Name == idioma.ToLower()
                             select tipo.Name).FirstOrDefault();

        return nombreTipo;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idPokemon"></param>
    /// <returns></returns>
    //Metodo para probar los obtener la imagen del pokemon y poder probar pasarlo a sprite de la imagen
    public static byte[] getImageFrentePokemon(int idPokemon)
    {
        byte[] imagenPokemon = null;

        SqlConnection conexion = Conexion.establecerConexion();
        SqlCommand command = new SqlCommand("Select ImagenDeFrente FROM ImagenesPokemons WHERE IDPokemon = @ID", conexion);
        command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = idPokemon;
        SqlDataReader reader = command.ExecuteReader();
        reader.Read();

        imagenPokemon = (byte[])reader.GetValue(0);

        return imagenPokemon;
    }
}
