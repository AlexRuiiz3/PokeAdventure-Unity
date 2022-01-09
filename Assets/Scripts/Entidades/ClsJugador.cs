using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClsJugador
{
    public ClsJugador() {
        NombreUsuario = "";
        Contrasenha = "";
        CorreoElectronico = "";
        NivelCuenta = 0;
        Experiencia= 0;
        Foto = new byte[0];
    }

    public ClsJugador(string nombreUsuario, string contrasenha, string correoElectronico, int nivelCuenta, int experiencia, byte[] foto)
    {
        NombreUsuario = nombreUsuario;
        Contrasenha = contrasenha;
        CorreoElectronico = correoElectronico;
        NivelCuenta = nivelCuenta;
        Experiencia = experiencia;
        Foto = foto;
    }

    public string NombreUsuario { get; set; }
    public string Contrasenha { get; set; }
    public string CorreoElectronico { get; set; }
    public int NivelCuenta { get; set; }
    public int Experiencia { get; set; }
    public byte[] Foto { get; set; }

}
