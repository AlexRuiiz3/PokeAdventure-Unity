using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UtilidadesDal
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <returns></returns>
    public static bool comprobarSiExisteNombreUsuario(string nombreUsuario) {
        bool existeNombreUsuario = false;
        SqliteConnection conexion = null;
        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            SqliteCommand command = new SqliteCommand("SELECT NombreUsuario FROM Jugadores WHERE NombreUsuario = @NombreUsuario", conexion);
            command.Parameters.Add("@NombreUsuario",System.Data.DbType.String).Value = nombreUsuario;

            SqliteDataReader reader = command.ExecuteReader();
            existeNombreUsuario = reader.HasRows;
        }
        catch (Exception)
        {
            throw;
        }
        finally {
            ConfiguracionDB.cerrarConexion(conexion);
        }

        return existeNombreUsuario;
    }
}

