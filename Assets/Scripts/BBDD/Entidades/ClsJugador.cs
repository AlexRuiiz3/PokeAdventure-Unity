using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClsJugador
{
    public ClsJugador()
    {
        ID = 0;
        NombreUsuario = "";
        Contrasenha = "";
        CorreoElectronico = "";
        NivelCuenta = 0;
        Experiencia = 0;
        Dinero = 0;
        Foto = new byte[0];
    }

    public ClsJugador(int id, string nombreUsuario, string contrasenha, string correoElectronico, int nivelCuenta, int experiencia, int dinero, byte[] foto)
    {
        ID = id;
        NombreUsuario = nombreUsuario;
        Contrasenha = contrasenha;
        CorreoElectronico = correoElectronico;
        NivelCuenta = nivelCuenta;
        Experiencia = experiencia;
        Dinero = dinero;
        Foto = foto;
    }

    public int ID { get; }
    public string NombreUsuario { get; }
    public string Contrasenha { get; }
    public string CorreoElectronico { get; set; }
    public int NivelCuenta { get; set; }
    public int Experiencia { get; set; }
    public int Dinero { get; set; }
    public byte[] Foto { get; set; }

}
