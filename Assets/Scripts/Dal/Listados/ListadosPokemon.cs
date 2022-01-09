using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class ListadosPokemon 
{
    //Metodo para probar los obtener la imagen del pokemon y poder probar pasarlo a sprite de la imagen
    public static byte[] getImageFrentePokemon(int idPokemon) {
        byte[] imagenPokemon = null;

        SqlConnection conexion = Conexion.establecerConexion();
        SqlCommand command = new SqlCommand("Select ImagenDeFrente FROM ImagenesPokemons WHERE IDPokemon = @ID", conexion);
        command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = idPokemon;
        SqlDataReader reader = command.ExecuteReader();
        reader.Read();

        imagenPokemon = (byte[])reader.GetValue(0);

        return imagenPokemon;
    }
}
