using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UtilidadesDal
{
    public static bool comprobarSiExisteNombreUsuario(string nombreUsuario) {
        bool existeNombreUsuario = false;
        SqlConnection conexion = null;
        try
        {
            conexion = Conexion.establecerConexion();
            SqlCommand command = new SqlCommand("SELECT NombreUsuario FROM Jugadores WHERE NombreUsuario = @NombreUsuario", conexion);
            command.Parameters.Add("@NombreUsuario",System.Data.SqlDbType.VarChar).Value = nombreUsuario;

            SqlDataReader reader = command.ExecuteReader();
            existeNombreUsuario = reader.HasRows;
        }
        catch (Exception)
        {
            throw;
        }
        finally {
            Conexion.cerrarConexion(conexion);
        }

        return existeNombreUsuario;
    }
}

