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
    /// Cabecera: public static bool comprobarSiExisteNombreUsuario(string nombreUsuario)
    /// Comentario: Este metodo se encarga de comprobar en la base de datos si existe un jugador con un nombre de usuario determinado.
    /// Entradas: string nombreUsuario
    /// Salidas: bool existe
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un dato booleano cuyo valor puede ser:
    ///                  true: Cuando en la base de datos exista algun jugador cuyo nombre de usuario sea igual al recibido.
    ///                  false: Cuando en la base de datos no exista ningun jugador cuyo nombre de usuario sea igual al recibido o cuando se produzca cualquier 
    ///                         tipo de excepcion.
    /// </summary>
    /// <param name="nombreUsuario"></param>
    /// <returns>bool</returns>
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
        }
        finally {
            ConfiguracionDB.cerrarConexion(conexion);
        }
        return existeNombreUsuario;
    }
}
