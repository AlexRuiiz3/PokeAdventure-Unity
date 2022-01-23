using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using PokeAPI;

public class ListadosPokemon
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
    public static async Task<List<string>> obtenerNombreTiposPokemon(List<PokemonType> tiposPokemon, string idioma)
    {
        PokeAPI.Language
        List<string> nombreTiposPokemon = new List<string>();
        string tipoIdioma;

        foreach (PokemonType tipoPokemon in tiposPokemon)
        {
            tipoIdioma = await obtenerNombreTipoPokemonEnUnIdioma(tipoPokemon.Type.Name,"es");
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
    public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(List<PokemonType> tiposPokemon, string idioma)
    {
        PokeApiClient apiClient = new PokeApiClient();
        List<string> nombreDebilidadesPokemon = new List<string>();
        string tipoDebilIdioma;
        Type tipo;

        foreach (PokemonType tipoPokemon in tiposPokemon)
        {
            tipo = await apiClient.GetResourceAsync<Type>(tipoPokemon.Type.Url);
            foreach (NamedApiResource<Type> tipoFuerte in tipo.DamageRelations.DoubleDamageFrom)
            {
                tipoDebilIdioma = await obtenerNombreTipoPokemonEnUnIdioma(tipoFuerte.Name, "es");
                nombreDebilidadesPokemon.Add(tipoDebilIdioma);
            }
        }
        return nombreDebilidadesPokemon;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    public static async Task<string> obtenerNombreTipoPokemonEnUnIdioma(string nombre, string idioma)
    {

        PokeApiClient apiClient = new PokeApiClient();
        string tipoEnEspanhol;
        Type tipo = await apiClient.GetResourceAsync<Type>(nombre);

        tipoEnEspanhol = (from nombreTipo in tipo.Names
                          where nombreTipo.Language.Name == idioma
                          select nombreTipo.Name).First();

        return tipoEnEspanhol;
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
