using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

public class GestoraPokemonsJugadorDAL
{
    public static void guardarPokemonsDeJugador(int idJugador,List<PokemonJugador> pokemons)
    {
        SqliteConnection conexion = null;
        try
        {
            string commandText = prepararCommandInsertarPokemonsJugador(pokemons);
            conexion = ConfiguracionDB.establecerConexion();
            SqliteCommand command = new SqliteCommand(commandText, conexion);
            command.Parameters.Add("@IDJugador", System.Data.DbType.UInt32).Value = idJugador;
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

    private static string prepararCommandInsertarPokemonsJugador(List<PokemonJugador> pokemons) {
        string commandEliminarText = $"DELETE FROM PokemonsJugadores WHERE IDJugador = @IDJugador " +
            $" AND NumeroPokemon NOT IN ( ",
            commandInsertarText = "",
            commandInsertMovimientos = "",
            commandInsertPokemonsMovimientos = "",
            commandInsertTiposPokemon = "";
        foreach (PokemonJugador pokemon in pokemons) {
            commandInsertarText += $"INSERT OR REPLACE INTO PokemonsJugadores (IDJugador,IDPokemon,NumeroPokemon,Nombre,HP,HPMaximos,Nivel,Ataque,Defensa,Velocidad,NumeroEquipado,Experiencia,ExperienciaSiguienteNivel) VALUES(@IDJugador,{pokemon.ID},{pokemon.PokemonNumero},'{pokemon.Nombre}',{pokemon.HP},{pokemon.HPMaximos},{pokemon.Nivel},{pokemon.Ataque},{pokemon.Defensa},{pokemon.Velocidad},{pokemon.NumeroEquipado},{pokemon.Experiencia},{pokemon.ExperienciaSiguienteNivel}); ";
            commandEliminarText += $"'{pokemon.PokemonNumero}',";
            foreach (MovimientoPokemon movimiento in pokemon.Movimientos) {
                commandInsertMovimientos += $"INSERT OR REPLACE INTO Movimientos (Nombre,Danho,Precision,PP,Tipo) VALUES ('{movimiento.Nombre}',{movimiento.Danho},{movimiento.Precicion},{movimiento.PP},(SELECT ID FROM Tipos WHERE Nombre = '{movimiento.Tipo}')); ";
                commandInsertPokemonsMovimientos += $"INSERT OR REPLACE INTO PokemonsJugadoresMovimientos  (IDJugador,IDPokemon,NumeroPokemon,IDMovimiento) VALUES(@IDJugador,{pokemon.ID},{pokemon.PokemonNumero},(SELECT MT FROM Movimientos WHERE Nombre = '{movimiento.Nombre}')); ";
            }
            foreach (string tipo in pokemon.Tipos) {
                commandInsertTiposPokemon += $"INSERT OR REPLACE INTO TiposPokemons (IDPokemon,IDTipo) VALUES({pokemon.ID},(SELECT ID FROM Tipos WHERE Nombre = '{tipo}')); ";
            }
        }
        commandEliminarText = commandEliminarText.Remove(commandEliminarText.Length - 1);//Se elimina la ultima coma
        commandEliminarText += $"); {commandInsertarText} {commandInsertMovimientos} {commandInsertPokemonsMovimientos} {commandInsertTiposPokemon}";
        return commandEliminarText;
    }
}

