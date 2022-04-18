using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosPokemonsJugadorDAL
{

    public static List<PokemonJugador> obtenerPokemonsNoEquipadosJugador(int id) 
    {
        List<PokemonJugador> pokemonsJugador = new List<PokemonJugador>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;
        
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM PokemonsJugadores WHERE IDJugador = @ID AND NumeroEquipado == 0;", conexion);
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
                    foreach (string tipo in tiposPokemon) {
                        debilidadesPokemon.Concat(ListadosTipoBL.obtenerTiposDebilesTipo(tipo));
                    }
                    debilidadesPokemon = debilidadesPokemon.Distinct().ToList();//Se eliminan los tipos repetidos que existan.Por ejemplo pokemon tipo planta e hielo tendria duplicado el tipo debilad fuego, se elimina ya que ocuparia un espacio innecesario
                    
                    
                    pokemonsJugador.Add(new PokemonJugador(
                        new Pokemon(reader.GetInt32(1), reader.GetString(3), reader.GetInt32(4),
                        reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8),movimientosPokemon,tiposPokemon,debilidadesPokemon, new byte[0], new byte[0])
                        , reader.GetInt32(0), reader.GetInt32(2), reader.GetInt32(9), reader.GetInt32(10)
                        ));
                }
            }
        }
        catch (Exception)
        {
            Debug.Log("Error en la obtencion de los pokemons no equipados");
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
    
    public static List<PokemonJugador> obtenerPokemonsJugadorEquipados(int id)
    {
        List<PokemonJugador> pokemonsJugador = new List<PokemonJugador>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM PokemonsJugadores WHERE IDJugador = @ID AND NumeroEquipado != 0;", conexion);
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
                        reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), movimientosPokemon, tiposPokemon, debilidadesPokemon, new byte[0], new byte[0])
                        , reader.GetInt32(0), reader.GetInt32(2), reader.GetInt32(9), reader.GetInt32(10)
                        ));
                }
            }
        }
        catch (Exception)
        {
            throw;
            Debug.Log("Error en la obtencion de los pokemons equipados");
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


}

