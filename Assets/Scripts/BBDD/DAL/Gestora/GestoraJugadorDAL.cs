using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoraJugadorDAL
{
    public static void insertarJugador(ClsJugador jugador)
    {
        SqliteConnection conexion = null;
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            SqliteCommand command = new SqliteCommand("INSERT INTO Jugadores (NombreUsuario,Contrasenha,CorreoElectronico,Dinero,Foto) VALUES(@NombreUsuario,@Contrasenha,@CorreoElectronico,@Dinero,NULL)", conexion);
            command.Parameters.Add("@NombreUsuario", System.Data.DbType.String).Value = jugador.NombreUsuario;
            command.Parameters.Add("@Contrasenha", System.Data.DbType.String).Value = jugador.Contrasenha;
            command.Parameters.Add("@CorreoElectronico", System.Data.DbType.String).Value = jugador.CorreoElectronico;
            command.Parameters.Add("@Dinero", System.Data.DbType.Int32).Value = jugador.Dinero;
            //De momento a null command.Parameters.Add("@Foto",System.Data.DbType.VarBinary).Value = jugador.Foto;
            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
        }
    }

    public static int actualizarDineroJugador(int id, int dinero)
    {
        int actualizaciones = 0;
        SqliteConnection conexion = null;
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            SqliteCommand command = new SqliteCommand("UPDATE Jugadores SET Dinero = @Dinero WHERE ID = @ID", conexion);
            command.Parameters.Add("@ID", System.Data.DbType.Int32).Value = id;
            command.Parameters.Add("@Dinero", System.Data.DbType.Int32).Value = dinero;
            actualizaciones = command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Debug.Log("Error en la actualizacion del dato dinero del jugador");//throw;
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
        }
        return actualizaciones;
    }
}
