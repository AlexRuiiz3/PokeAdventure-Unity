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
    /// 
    /// </summary>
    /// <returns></returns>
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
            Debug.Log("Error en la obtencion de los datos de un item aleatorio");
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
    /// 
    /// </summary>
    /// <returns></returns>
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
            Debug.Log("Error en la obtencion de los datos de los items");
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
            throw;
            Debug.Log("Error en la obtencion de los datos de los items del jugador");
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
            Debug.Log("Error en la obtencion de los datos de un item por nombre");
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

