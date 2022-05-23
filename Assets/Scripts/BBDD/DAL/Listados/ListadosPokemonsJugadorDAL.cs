using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosPokemonsJugadorDAL
{
    /// <summary>
    /// Cabecera: public static List<PokemonJugador> obtenerPokemonsNoEquipadosJugador(int id)
    /// Comentario: Este metodo se encarga de obtener los pokemons de un jugador que no tiene equipados.
    /// Entradas: int id
    /// Salidas: List<PokemonJugador> pokemonsJugador
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista con objetos de tipo PokemonJugador. Si se produce una excepcion o no se encuentra un jugador con el id 
    ///                  recibido o la consulta no tiene resultados, la lista devuelta estara vacia. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<PokemonJugador></returns>
    public static List<PokemonJugador> obtenerPokemonsNoEquipadosJugador(int id) 
    {
        List<PokemonJugador> pokemonsJugador = new List<PokemonJugador>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;
        
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM PokemonsJugadores WHERE IDJugador = @ID AND NumeroEquipado == 0 ORDER BY NumeroPokemon;", conexion);
            command.Parameters.Add("@ID", System.Data.DbType.UInt32).Value = id;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                List<MovimientoPokemon> movimientosPokemon;
                List<string> tiposPokemon;
                List<string> debilidadesPokemon = new List<string>();
                while (reader.Read())
                {
                    //Se obtienen los movimientos del pokemon
                    movimientosPokemon = ListadosMovimientoBL.obtenerMovimientosPokemon(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
                    //Se obtienen los tipos del pokemon
                    tiposPokemon = ListadosTipoBL.obtenerTiposPokemon(reader.GetInt32(1));
                    //Por cada tipo del pokemon se obtienen sus tipos debiles
                    foreach (string tipo in tiposPokemon) {
                        debilidadesPokemon.Concat(ListadosTipoBL.obtenerTiposDebilesTipo(tipo));
                    }
                    debilidadesPokemon = debilidadesPokemon.Distinct().ToList();//Se eliminan los tipos repetidos que existan.Por ejemplo pokemon tipo planta e hielo tendria duplicado el tipo debilad fuego, se elimina ya que ocuparia un espacio innecesario
                    
                    pokemonsJugador.Add(new PokemonJugador(
                        new Pokemon(reader.GetInt32(1), reader.GetString(3), reader.GetInt32(4),
                        reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8),movimientosPokemon,tiposPokemon,debilidadesPokemon)
                        , reader.GetInt32(0), reader.GetInt32(2), reader.GetInt32(9), reader.GetInt32(10)
                        ));
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return pokemonsJugador;
    }
    
    /// <summary>
    /// Cabecera: public static List<PokemonJugador> obtenerPokemonsJugadorEquipados(int id)
    /// Comentario: Este metodo se encarga de obtener los pokemons de un jugador que tiene equipados.
    /// Entradas: int id
    /// Salidas: List<PokemonJugador> pokemonsJugador
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista con objetos de tipo PokemonJugador. Si se produce una excepcion o no se encuentra un jugador con el id 
    ///                  recibido o la consulta no tiene resultados, la lista devuelta estara vacia. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<PokemonJugador></returns>
    public static List<PokemonJugador> obtenerPokemonsJugadorEquipados(int id)
    {
        List<PokemonJugador> pokemonsJugador = new List<PokemonJugador>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM PokemonsJugadores WHERE IDJugador = @ID AND NumeroEquipado != 0 ORDER BY NumeroEquipado; ", conexion);
            command.Parameters.Add("@ID", System.Data.DbType.UInt32).Value = id;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                List<MovimientoPokemon> movimientosPokemon;
                List<string> tiposPokemon;
                List<string> debilidadesPokemon = new List<string>();
                while (reader.Read())
                {
                    movimientosPokemon = ListadosMovimientoBL.obtenerMovimientosPokemon(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
                    tiposPokemon = ListadosTipoBL.obtenerTiposPokemon(reader.GetInt32(1));
                    foreach (string tipo in tiposPokemon)
                    {
                        debilidadesPokemon.AddRange(ListadosTipoBL.obtenerTiposDebilesTipo(tipo));
                    }
                    debilidadesPokemon = debilidadesPokemon.Distinct().ToList();//Se eliminan los tipos repetidos que existan.Por ejemplo pokemon tipo planta e hielo tendria duplicado el tipo debilad fuego, se elimina ya que ocuparia un espacio innecesario

                    pokemonsJugador.Add(new PokemonJugador(
                        new Pokemon(reader.GetInt32(1), reader.GetString(3), reader.GetInt32(4),
                        reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), movimientosPokemon, tiposPokemon, debilidadesPokemon)
                        , reader.GetInt32(0), reader.GetInt32(2), reader.GetInt32(9), reader.GetInt32(10)
                        ));
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return pokemonsJugador;
    }

    /// <summary>
    /// Cabecera: public static int obtenerNumeroPokemonsJugador(int id)
    /// Comentario: Este metodo se encarga de obtener el numero total de pokemons que tiene un jugador en especifico
    /// Entradas: int id
    /// Salidas: int numeroPokemons
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un entero con el numero de pokemons que tiene un jugador. Si se produce una excepcion o no se encuentra un jugador con el id 
    ///                  recibido o la consulta no tiene resultados, se devolvera 0. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>int</returns>
    public static int obtenerNumeroPokemonsJugador(int id)
    {
        int numeroPokemons = 0;
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT COUNT(*) FROM PokemonsJugadores WHERE IDJugador = @ID GROUP BY IDJugador;", conexion);
            command.Parameters.Add("@ID", System.Data.DbType.Int32).Value = id;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                numeroPokemons = reader.GetInt32(0);
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return numeroPokemons;
    }
}
