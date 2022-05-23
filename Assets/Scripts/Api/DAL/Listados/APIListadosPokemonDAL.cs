using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System;
using PokeAPI;

public class APIListadosPokemonDAL
{

    /// <summary>
    /// Cabecera: public static async Task<PokeAPI.Pokemon> obtenerPokemonDeApi(int id)
    /// Comentario: Este metodo se encarga de obtener de una api(PokeAPi) un pokemon especifico 
    /// Entradas: int id
    /// Salidas: PokeAPI.Pokemon  
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo PokeAPI.Pokemon. Si el id no coincide con el de ningun pokemon, se devolvera null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>PokeAPI.Pokemon</returns>
    public static async Task<PokeAPI.Pokemon> obtenerPokemonDeApi(int id)
    {
        return await DataFetcher.GetApiObject<PokeAPI.Pokemon>(id);
    }
    
    /// <summary>
    /// Cabecera: public static async Task<List<string>> obtenerNombreTiposPokemon(PokemonTypeMap[] tiposPokemon)
    /// Comentario: Este metodo se encarga de obtener los tipos(Solo el nombre) de un pokemon.
    /// Entradas: PokemonTypeMap[] tiposPokemon
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de string que tendra como valores los tipos de un pokemon. Si se produce algun error, se devolvera la lista vacia.
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <returns>List<string></returns>
    public static async Task<List<string>> obtenerNombreTiposPokemon(PokemonTypeMap[] tiposPokemon)
    {
        List<string> nombreTiposPokemon = new List<string>();
        PokemonType tipo;
        string tipoIdioma;

        foreach (PokemonTypeMap tipoPokemon in tiposPokemon)
        {
            tipo = await DataFetcher.GetApiObject<PokemonType>(tipoPokemon.Type.ID);
            tipoIdioma = obtenerNombreEnUnIdioma(tipo.Names);
            nombreTiposPokemon.Add(tipoIdioma);
        }
        return nombreTiposPokemon;
    }
    /// <summary>
    /// Cabecera: public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(PokemonTypeMap[] tiposPokemon)
    /// Comentario: Este metodo se encarga de obtener los tipos debiles de un pokemon.
    /// Entradas: PokemonTypeMap[] tiposPokemon
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de string que tendra como valores los tipos de un pokemon. Si se produce algun error, se devolvera la lista vacia.
    /// </summary>
    /// <param name="tiposPokemon"></param>
    /// <returns>List<string></returns>
    public static async Task<List<string>> obtenerNombreTiposDebilesPokemon(PokemonTypeMap[] tiposPokemon)
    {
        List<string> listadosTiposDobleDanho = new List<string>();
        PokemonType tipo;
        PokemonType tipoDanho;
        string tipoIdioma;

        foreach (PokemonTypeMap tipoPokemon in tiposPokemon)
        {
            tipo = await DataFetcher.GetApiObject<PokemonType>(tipoPokemon.Type.ID);
            foreach (NamedApiResource<PokemonType> tipoDobleDanho in tipo.DamageRelations.DoubleDamageFrom)
            {
                tipoDanho = await DataFetcher.GetApiObject<PokemonType>(tipoDobleDanho.ID);
                tipoIdioma = obtenerNombreEnUnIdioma(tipoDanho.Names);
                listadosTiposDobleDanho.Add(tipoIdioma);
            }

        }
        return listadosTiposDobleDanho;
    }

    public static async Task<List<MovimientoPokemon>> obtenerMovimientosAleatoriosPokemon(PokemonMove[] pokemonMoves)
    {
        List<Move> listadoMovimientos = new List<Move>();
        List<PokemonMove> movimientosSeleccionados = new List<PokemonMove>();
        Move movimiento;
        int aleatorio;
        System.Random random = new System.Random();

        while (listadoMovimientos.Count < 4)//4 Porque son 4 los movimientos que se quieren obtener 
        {
            aleatorio = random.Next(pokemonMoves.Length - 1);
            if (!comprobarMovimientoYaObtenido(listadoMovimientos, pokemonMoves[aleatorio].Move.ID))//Si el movimiento obtenido de forma aleatoria no se ha seleccionado antes
            {
                movimiento = await DataFetcher.GetApiObject<Move>(pokemonMoves[aleatorio].Move.ID); //Hay que obtener de la api el objeto de tip Move, ya que tiene toda la inforacion de los movimientos, y asi se puede comprobar si es un movimiento que causa daño, ya que seran los unicos que se obtendran
                if (movimiento.Power != null && movimiento.Power > 0 && movimiento.Accuracy > 0)
                {
                    listadoMovimientos.Add(movimiento);
                }
            }
        }
        return await mappearListaMoveAMovimientoPokemon(listadoMovimientos);
    }

    private static async Task<List<MovimientoPokemon>> mappearListaMoveAMovimientoPokemon(List<Move> movesPokemon)
    {
        List<MovimientoPokemon> listadoMovimientos = new List<MovimientoPokemon>();
        PokemonType tipo;
        string nombreMovimientoEnIdioma;
        string nombreTipoEnIdioma;
        foreach (Move move in movesPokemon)
        {
            tipo = await DataFetcher.GetApiObject<PokemonType>(move.Type.ID);
            nombreTipoEnIdioma = obtenerNombreEnUnIdioma(tipo.Names);
            nombreMovimientoEnIdioma = obtenerNombreEnUnIdioma(move.Names);
            listadoMovimientos.Add(new MovimientoPokemon(move.ID, nombreMovimientoEnIdioma, (int)move.Power, (int)move.Accuracy, (int)move.PP, nombreTipoEnIdioma));
        }
        return listadoMovimientos;
    }
    private static bool comprobarMovimientoYaObtenido(List<Move> movesPokemon, int idMovimiento)
    {
        bool obtenido = false;

        for (int i = 0; i < movesPokemon.Count && !obtenido; i++)
        {
            if (movesPokemon[i].ID == idMovimiento)
            {
                obtenido = true;
            }
        }
        return obtenido;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="idioma"></param>
    /// <returns></returns>
    private static string obtenerNombreEnUnIdioma(ResourceName[] listadoNombres)
    {
        string nombre = (from nombreSeleccionado in listadoNombres
                         where nombreSeleccionado.Language.Name == PlayerPrefs.GetString("GameLanguage").ToLower()
                         select nombreSeleccionado.Name).FirstOrDefault();

        return nombre;
    }
    /*
    public static async Task asignarImagenesPokemonsJugador(List<PokemonJugador> pokemonsJugador) {
        PokeAPI.Pokemon pokemonApi;
        string urlSpriteFrente;
        string urlSpriteEspalda;
        foreach (PokemonJugador pokemonJugador in pokemonsJugador) {
            pokemonApi = await obtenerPokemonDeApi(pokemonJugador.ID);
            
            urlSpriteFrente = (pokemonApi.Sprites.FrontMale != null) ? pokemonApi.Sprites.FrontMale : pokemonApi.Sprites.FrontFemale;
            if (urlSpriteFrente != null) //si no tiene ni hembra ni maculino se dejara vacio.
            {
                pokemonJugador.ImagenDeFrente = Utilidades.obtenerImagenDeUrl(urlSpriteFrente);
            }

            urlSpriteEspalda = (pokemonApi.Sprites.BackMale != null) ? pokemonApi.Sprites.BackMale : pokemonApi.Sprites.BackFemale;
            if (urlSpriteEspalda != null)
            {
                pokemonJugador.ImagenDeEspalda = Utilidades.obtenerImagenDeUrl(urlSpriteEspalda);
            }
        }
    }*/
}
