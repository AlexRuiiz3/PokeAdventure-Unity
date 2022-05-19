using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GestoraPokemonEncontradosJugadorDAL
{

    public static void insertarPokemonEncontradoAJugador(int idJugador, int idPokemon, string nombrePokemon)
    {
        SqliteConnection conexion = null;
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            SqliteCommand command = new SqliteCommand("INSERT INTO PokemonsEncontradosJugadores (IdJugador,IdPokemon,NombrePokemon)" +
                "SELECT @IdJugador, @IdPokemon, @NombrePokemon " +
                "WHERE NOT EXISTS(SELECT 1 FROM PokemonsEncontradosJugadores " +
                                 "WHERE IdJugador = @IdJugador AND IdPokemon = @IdPokemon); ", conexion);
            command.Parameters.Add("@IdJugador", System.Data.DbType.Int32).Value = idJugador;
            command.Parameters.Add("@IdPokemon", System.Data.DbType.Int32).Value = idPokemon;
            command.Parameters.Add("@NombrePokemon", System.Data.DbType.String).Value = nombrePokemon;
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

