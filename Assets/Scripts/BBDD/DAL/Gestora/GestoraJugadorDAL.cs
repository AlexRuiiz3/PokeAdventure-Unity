using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class GestoraJugadorDAL
{
    public static void insertarJugador(ClsJugador jugador)
    {
        SqlConnection conexion = null;
        try
        {
            conexion = Conexion.establecerConexion();
            SqlCommand command = new SqlCommand("INSERT INTO Jugadores VALUES(@NombreUsuario,@Contrasenha,@CorreoElectronico,@NivelCuenta,@Experiencia,@Dinero,@Foto)", conexion);
            command.Parameters.Add("@NombreUsuario",System.Data.SqlDbType.VarChar).Value = jugador.NombreUsuario;
            command.Parameters.Add("@Contrasenha",System.Data.SqlDbType.VarChar).Value = jugador.Contrasenha;
            command.Parameters.Add("@CorreoElectronico",System.Data.SqlDbType.VarChar).Value = jugador.CorreoElectronico;
            command.Parameters.Add("@NivelCuenta",System.Data.SqlDbType.SmallInt).Value = jugador.NivelCuenta;
            command.Parameters.Add("@Experiencia",System.Data.SqlDbType.Int).Value = jugador.Experiencia;
            command.Parameters.Add("@Dinero",System.Data.SqlDbType.Int).Value = jugador.Dinero;
            command.Parameters.Add("@Foto",System.Data.SqlDbType.VarBinary).Value = jugador.Foto;

            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally {
            Conexion.cerrarConexion(conexion);
        }
    }
}
