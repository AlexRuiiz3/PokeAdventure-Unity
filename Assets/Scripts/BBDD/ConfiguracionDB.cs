using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfiguracionDB
{
    private static string nombreDB = "URI=file:PokeAdventure.db";

    /// <summary>
    /// Cabecera: public static SqliteConnection establecerConexion()
    /// Comentario: Este metodo se encarga de abrir una conexion con la base de datos.
    /// Entradas: Ninguna
    /// Salidas: SqliteConnection conexion
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo SqliteConnection, con la conexion abierta a la base de datos.
    /// </summary>
    /// <returns>SqliteConnection</returns>
    public static SqliteConnection establecerConexion()
    {
        SqliteConnection conexion = new SqliteConnection(nombreDB);
        conexion.Open();
        return conexion;
    }
    /// <summary>
    /// Cabecera: public static void cerrarConexion(SqliteConnection conexion)
    /// Comentario: Este metodo se encarga de cerrar una la conexion de un objeto de tipo SqliConnection
    /// Entradas: SqliteConnection conexion
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se cerrara la conexion del objeto SqliteConnection 
    /// </summary>
    /// <param name="conexion"></param>
    /// <returns></returns>
    public static void cerrarConexion(SqliteConnection conexion)
    {
        if (conexion != null)
        {
            conexion.Close();
        }
    }
    /// <summary>
    /// Cabecera: public static void createDB()
    /// Comentario: Este metodo se encarga de crear la base de datos a partir del script que se encuentra en un fichero
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se creara la base de datos. Si se produce una excepcion la base de datos no se creara 
    /// </summary>
    /// <returns></returns>
    public static void createDB()
    {
            SqliteConnection conexion = null;
            try
            {
                conexion = establecerConexion();
                string contenidoScript = File.ReadAllText("Assets\\Plugins\\PokeAdventureBD.sql");
                SqliteCommand sqlCommand = new SqliteCommand(contenidoScript,conexion);
                sqlCommand.ExecuteNonQuery();
                PlayerPrefs.SetInt("BaseDatosCreada",1);
            }
            catch (Exception)
            {
            throw;
            }
            finally
            {
                cerrarConexion(conexion);
            }
    }
}
