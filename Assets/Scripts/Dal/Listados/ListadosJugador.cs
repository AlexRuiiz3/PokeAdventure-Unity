using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class ListadosJugador
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <param name="contrasenha"></param>
    /// <returns></returns>
    public static int obtenerIDJugador(string nombreUsuario, string contrasenha) {
        int idJugador = -1;
        SqlConnection sqlConexion = null;

        try
        {
            sqlConexion = Conexion.establecerConexion();
            SqlCommand sqlCommand = new SqlCommand("SELECT ID FROM Jugadores " +
                "WHERE NombreUsuario = @NombreUsuario AND Contrasenha = @Contrasenha",sqlConexion);
            sqlCommand.Parameters.Add("@NombreUsuario",System.Data.SqlDbType.VarChar).Value = nombreUsuario;
            sqlCommand.Parameters.Add("@Contrasenha",System.Data.SqlDbType.VarChar).Value = contrasenha;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            if(sqlDataReader.HasRows){
                sqlDataReader.Read();
                idJugador = sqlDataReader.GetInt16(0);
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally {
            Conexion.cerrarConexion(sqlConexion); //El metodo controla cuando la conexion es null
        }
        return idJugador;
    }
}
