using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que hace referencia a la tabla Jugadores de la base de datos
public class ClsJugador
{

    private int dinero;
    #region Constructores
    //Constructor sin parametros
    public ClsJugador()
    {
        ID = 0;
        NombreUsuario = "";
        Contrasenha = "";
        CorreoElectronico = "";
        Dinero = 0;
        Foto = new byte[0];
    }

    //Constructor con parametros
    public ClsJugador(int id, string nombreUsuario, string contrasenha, string correoElectronico, int dinero, byte[] foto)
    {
        ID = id;
        NombreUsuario = nombreUsuario;
        Contrasenha = contrasenha;
        CorreoElectronico = correoElectronico;
        this.dinero = dinero;
        Foto = foto;
    }
    #endregion

    #region Metodos fundamentales
    //id
    public int ID { get; }
    //nombreUsuario
    public string NombreUsuario { get; }
    //contrasenha
    public string Contrasenha { get; }
    //correoElectronico
    public string CorreoElectronico { get; set; }
    //dinero
    public int Dinero
    {
        get { return dinero; }
        set
        {
            if (value < 0)
            {
                dinero = 0;
            }
            else
            {
                dinero = value;
            }

        }
    }
    //foto
    public byte[] Foto { get; set; }
    #endregion
}
