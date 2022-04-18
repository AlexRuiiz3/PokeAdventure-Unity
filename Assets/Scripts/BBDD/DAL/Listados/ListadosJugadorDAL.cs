using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class ListadosJugadorDAL
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <returns></returns>
    public static bool comprobarExistenciaNombreUsuario(string nombreUsuario)
    {
        bool existe = true;
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM Jugadores WHERE NombreUsuario = @NombreUsuario", conexion);
            command.Parameters.Add("@NombreUsuario", System.Data.DbType.String).Value = nombreUsuario;
            reader = command.ExecuteReader();

            existe = reader.HasRows;
        }
        catch (Exception)
        {
            Debug.Log("Error en la comprobacion de la existencia del nombre de usuario");
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return existe;
    }

    public static bool comprobarExistenciaNombreUsuarioContrasenha(string nombreUsuario, string contrasenha)
    {
        bool existe = true;
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM Jugadores WHERE NombreUsuario = @NombreUsuario AND Contrasenha = @Contrasenha", conexion);
            command.Parameters.Add("@NombreUsuario", System.Data.DbType.String).Value = nombreUsuario;
            command.Parameters.Add("@Contrasenha", System.Data.DbType.String).Value = contrasenha;
            reader = command.ExecuteReader();

            existe = reader.HasRows;
        }
        catch (Exception)
        {
            Debug.Log("Error en la comprobacion de la existencia del nombre de usuario");
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return existe;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <param name="contrasenha"></param>
    /// <returns></returns>
    public static ClsJugador obtenerJugador(string nombreUsuario, string contrasenha)
    {
        ClsJugador jugador = null;
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM Jugadores " +
                "WHERE NombreUsuario = @NombreUsuario AND Contrasenha = @Contrasenha;", conexion);
            command.Parameters.Add("@NombreUsuario",System.Data.DbType.String).Value = nombreUsuario;
            command.Parameters.Add("@Contrasenha",System.Data.DbType.String).Value = contrasenha;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                jugador = new ClsJugador(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    null //De momento a null la foto
                    ); 
            }
        }
        catch (Exception)
        {
            Debug.Log("Error en la obtencion de los datos del jugador OBTENER JUAGDOR");
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return jugador;
    }
}
