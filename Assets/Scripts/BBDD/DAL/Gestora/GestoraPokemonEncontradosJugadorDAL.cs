using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GestoraPokemonEncontradosJugadorDAL
{
    /// <summary>
    /// Cabecera: public static void insertarPokemonsEncontradosAJugador(int idJugador, List<PokemonEncontrado> pokemonsEncontrados)
    /// Comentario: Este metodo se encarga de guardar en la base datos un los pokemons que un jugador sea haya encontrado.
    /// Entradas: int idJugador, List<PokemonEncontrado> pokemonsEncontrados
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se insertara en la base de datos registros en la tabla PokemonsEncontradosJugadores que seran los pokemons encontrados que se encuentra un juagdor.
    ///                  Si se produce algun tipo de excepcion no se realizara ninguna insercion en la base de datos.
    /// </summary>
    /// <param name="idJugador"></param>
    /// <param name="pokemonsEncontrados"></param>
    /// <returns></returns>
    public static void insertarPokemonsEncontradosAJugador(int idJugador, List<PokemonEncontrado> pokemonsEncontrados)
    {
        SqliteConnection conexion = null;
        string commandText = "";
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            foreach (PokemonEncontrado pokemon in pokemonsEncontrados) {
                commandText += "INSERT INTO PokemonsEncontradosJugadores (IdJugador,IdPokemon,NombrePokemon)" +
                $"SELECT @IdJugador, {pokemon.Id}, '{pokemon.Nombre}' " +
                $"WHERE NOT EXISTS(SELECT 1 FROM PokemonsEncontradosJugadores WHERE IdJugador = @IdJugador AND IdPokemon = {pokemon.Id}); ";
            }

            SqliteCommand command = new SqliteCommand(commandText, conexion);
            command.Parameters.Add("@IdJugador", System.Data.DbType.Int32).Value = idJugador;
            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
        }
    }
}

