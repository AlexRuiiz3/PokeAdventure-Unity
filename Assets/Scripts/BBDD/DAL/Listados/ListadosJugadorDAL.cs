using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class ListadosJugadorDAL
{
    /// <summary>
    /// Cabecera: public static bool comprobarExistenciaNombreUsuarioContrasenha(string nombreUsuario, string contrasenha)
    /// Comentario: Este metodo se encarga de comprobar en la base de datos si existe un jugador con un nombre de usuario y contraseña determinados.
    /// Entradas: string nombreUsuario, string contrasenha
    /// Salidas: bool existe
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un dato booleano cuyo valor puede ser:
    ///                  true: Cuando en la base de datos exista algun jugador cuyo nombre de usuario y contraseña sean iguales a los recibidos.
    ///                  false: Cuando en la base de datos no exista ningun jugador cuyo nombre de usuario y contrasela sean iguales a los recibidos o cuando se produzca cualquier 
    ///                         tipo de excepcion.
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <param name="contrasenha"></param>
    /// <returns>bool</returns>
    public static bool comprobarExistenciaNombreUsuarioContrasenha(string nombreUsuario, string contrasenha)
    {
        bool existe = false;
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
    /// Cabecera: public static ClsJugador obtenerJugador(string nombreUsuario, string contrasenha)
    /// Comentario: Este metodo se encarga de obtener un objeto de tipo Jugador especifo de la base de datos segun el nombre de usuario y la contraseña.
    /// Entradas: string nombreUsuario, string contrasenha
    /// Salidas: ClsJugador jugador
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo Jugador. Si se produce alguna excepcion o no se encuentra ningun jugador cuyo nombre de usuario 
    //                   y contraseña sean iguales a los recibidos, se devolvera null. 
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <param name="contrasenha"></param>
    /// <returns>ClsJugador</returns>
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
