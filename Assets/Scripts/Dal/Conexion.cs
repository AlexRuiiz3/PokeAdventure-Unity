using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class Conexion 
{

    public static SqlConnection establecerConexion()
    {
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = "server=ALEXRUIZ\\SQLEXPRESS;" +
                                    "database=PokeAdventure;uid=UserPokeAdventure;pwd=pokeAdventure;";
        conexion.Open();
        return conexion;
    }
    /// <summary>
    /// Cabecera: public static void cerrarConexion(SqlConnection conexion)
    /// Comentario: Este metodo se encarga de cerrar la conexion de un objeto SQLConnection.
    /// Entradas: SqlConnection conexion
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: El objeto SqlConnection recibido cerrara su conexion, si esta a null no se cerrar el objeto SqlConnection.
    /// </summary>
    /// <param name="conexion"></param>
    public static void cerrarConexion(SqlConnection conexion)
    {
        if (conexion != null)
        {
            conexion.Close();
        }
    }
}
