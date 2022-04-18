using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfiguracionDB
{
    private static string nombreDB = "URI=file:PokeAdventure.db";

    public static SqliteConnection establecerConexion()
    {
        SqliteConnection conexion = new SqliteConnection(nombreDB);
        conexion.Open();
        return conexion;
    }

    public static void cerrarConexion(SqliteConnection conexion)
    {
        if (conexion != null)
        {
            conexion.Close();
        }
    }

    public static void createDB()
    {
            SqliteConnection conexion = null;
            try
            {
                conexion = establecerConexion();
                string contenidoScript = File.ReadAllText("Assets\\Scripts\\BBDD\\PokeAdventureBD.sql");
                SqliteCommand sqlCommand = new SqliteCommand(contenidoScript,conexion);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
            throw;
                //Debug.Log("Error en la creacion de la base de datos");
            }
            finally
            {
                cerrarConexion(conexion);
            }
    }
}
