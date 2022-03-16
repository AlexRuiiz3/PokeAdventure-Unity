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
    /// <param name="contrasenha"></param>
    /// <returns></returns>
    public static int obtenerIDJugador(string nombreUsuario, string contrasenha) {
        int idJugador = -1;
        SqlConnection conexion = null;

        try
        {
            conexion = Conexion.establecerConexion();
            SqlCommand command = new SqlCommand("SELECT ID FROM Jugadores " +
                "WHERE NombreUsuario = @NombreUsuario AND Contrasenha = @Contrasenha", conexion);
            command.Parameters.Add("@NombreUsuario",System.Data.SqlDbType.VarChar).Value = nombreUsuario;
            command.Parameters.Add("@Contrasenha",System.Data.SqlDbType.VarChar).Value = contrasenha;
            SqlDataReader sqlDataReader = command.ExecuteReader();

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
            Conexion.cerrarConexion(conexion); //El metodo controla cuando la conexion es null
        }
        return idJugador;
    }
}
