using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

public class GestoraPokemonsJugadorDAL
{
    /// <summary>
    /// Cabecera: public static void guardarPokemonsDeJugador(int idJugador,List<PokemonJugador> pokemons)
    /// Comentario: Este metodo se encarga de eliminar de la base de datos los pokemons de un jugador que ya no posea y añadir los que no existan en la base de datos y actualizar aquellos que si existan.
    /// Entradas: int idJugador,List<PokemonJugador> pokemons
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se realizaran operaciones de manipulacion(Insert,Delete,Update) sobre los pokemons de un jugador(PokemonJugador) en la base de datos. 
    ///                  No se realizara ninguna operacion en los siguientes casos:
    ///                  1: Si el id del jugador no existe en la base de datos o no es valido, no se realiara ninguna operacion. 
    ///                  2: Si la lista recibida esta vacia, no hay pokemons                        
    /// </summary>
    /// <param name="idJugador"></param>
    /// <param name="pokemons"></param>
    /// <returns></returns>
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
    /* Este metodo se encarga de preparar el string del command para insertar los pokemons pertenecientes a un jugador. Se prepara la operacion Delete para eliminar los pokemons que le jugador ya no tenga 
     * Tambien se prepararan los inserts para los movimientos del pokemon y de los tipos 
     */
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

