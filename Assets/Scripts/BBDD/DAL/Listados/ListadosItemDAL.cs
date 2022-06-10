using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListadosItemDAL
{

    /// <summary>
    /// Cabecera: public static Item obtenerItemAleatorio()
    /// Comentario: Este metodo se encarga de obtener de la base de datos un objeto de tipo Item de forma aleatoria
    /// Entradas: Ninguna
    /// Salidas: Item item
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un objeto de tipo Item de la base de datos de forma aleatoria.
    ///                  Si se produce alguna excepcion se devolvera un objeto de tipo Item con los valores por defecto.
    /// </summary>
    /// <returns>Item</returns>
    public static Item obtenerItemAleatorio()
    {
        Item item = new Item();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM Items ORDER BY RANDOM() LIMIT 1;", conexion);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                item = new Item(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    (reader.GetValue(3) == DBNull.Value) ? 0 : reader.GetInt32(3),
                    (reader.GetValue(4) == DBNull.Value) ? 0 : reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetString(6)
                    );
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return item;
    }

    /// <summary>
    /// Cabecera: public static List<Item> obtenerItems()
    /// Comentario: Este metodo se encarga de obtener todos los items que existan en la base de datos.
    /// Entradas: Ninguna
    /// Salidas: List<Item> item
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo Item que existen en la base de datos.
    ///                  Si se produce alguna excepcion se devolvera una lista de Item vacia.
    /// </summary>
    /// <returns>List<Item></returns>
    public static List<Item> obtenerItems()
    {
        List<Item> items = new List<Item>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM Items;", conexion);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new Item(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                (reader.GetValue(3) == DBNull.Value) ? 0 : reader.GetInt32(3),
                                (reader.GetValue(4) == DBNull.Value) ? 0 : reader.GetInt32(4),
                                reader.GetInt32(5),
                                reader.GetString(6)
                        ));
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return items;
    }

    /// <summary>
    /// Cabecera: public static List<ItemConCantidad> obtenerItemsJugador(int id)
    /// Comentario: Este metodo se encarga de obtener todos los items que tiene asociados un jugador.
    /// Entradas: int id
    /// Salidas: List<ItemConCantidad> items
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo ItemConCantidad que tiene un jugador especifico.
    ///                  Si se produce alguna excepcion o el id recibido no coincide con el de ningun jugador o el jugador no tiene ningun item asignado 
    ///                  se devolvera una lista de Item vacia.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List<ItemConCantidad></returns>
    public static List<ItemConCantidad> obtenerItemsJugador(int id)
    {
        List<ItemConCantidad> items = new List<ItemConCantidad>();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT I.*, IJ.Cantidad FROM Items AS I " +
                                        "INNER JOIN ItemsJugadores AS IJ ON I.ID = IJ.IDItem " +
                                        "WHERE IJ.IDJugador = @ID;", conexion);
            command.Parameters.Add("@ID",System.Data.DbType.UInt32).Value = id;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new ItemConCantidad(new Item(
                                                    reader.GetInt32(0),
                                                    reader.GetString(1),
                                                    reader.GetString(2),
                                                    (reader.GetValue(3) == DBNull.Value) ? 0 : reader.GetInt32(3),
                                                    (reader.GetValue(4) == DBNull.Value) ? 0 : reader.GetInt32(4),
                                                    reader.GetInt32(5),
                                                    reader.GetString(6)
                                                    ), 
                                                    reader.GetInt32(7))
                        );
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return items;
    }
    
    /// <summary>
    /// Cabecera: public static Item obtenerItem(int id)
    /// Comentario: Este metodo se encarga de obtener un item especifico de la base de datos.
    /// Entradas: int id
    /// Salidas: Item item
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera una lista de objetos de tipo ItemConCantidad que tiene un jugador especifico.
    ///                  Si se produce alguna excepcion o no se encuentra ningun item con el id recibido, se devolvera un item con los valores por defecto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Item</returns>
    public static Item obtenerItem(int id)
    {
        Item item = new Item();
        SqliteConnection conexion = null;
        SqliteCommand command;
        SqliteDataReader reader = null;

        try
        {
            conexion = ConfiguracionDB.establecerConexion();
            command = new SqliteCommand("SELECT * FROM Items WHERE id = @id;", conexion);
            command.Parameters.Add("@id",System.Data.DbType.String).Value = id;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                item = new Item(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    (reader.GetValue(3) == DBNull.Value) ? 0 : reader.GetInt32(3),
                    (reader.GetValue(4) == DBNull.Value) ? 0 : reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetString(6)
                    );
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ConfiguracionDB.cerrarConexion(conexion);
            if (reader != null)
            {
                reader.Close();
            }
        }
        return item;
    }
}

