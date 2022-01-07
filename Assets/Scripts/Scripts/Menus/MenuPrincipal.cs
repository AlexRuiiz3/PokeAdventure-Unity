using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    public InputField inputNombreUsuario;
    public InputField inputContrasenha;
    public InputField inputCorreoElectronico;

    public void iniciarSesion() {
        string nombreUsuario = inputNombreUsuario.text,
               contrasenha = inputContrasenha.text;
        int idJugador;

        if (!string.IsNullOrEmpty(nombreUsuario.Trim()) && !string.IsNullOrEmpty(contrasenha.Trim()))
        {
            try {
                idJugador = ListadosJugador.obtenerIDJugador(nombreUsuario, contrasenha);
                if (idJugador != -1) //-1 es el valor que tomara idJugador cuando en la base de datos no se encuentre un jugador en el que coincidan el usuario y contraseña especificados por el usuario
                {
                    SceneManager.LoadScene("LobbyScene");
                }
                else
                {
                    EditorUtility.DisplayDialog("Datos incorrectos", "Usuario o contraseña incorrectos", "Ok");
                }
            }
            catch (Exception) {
                EditorUtility.DisplayDialog("Error", "Ocurrio un error al intentar acceder a la base de datos", "Ok");
            }
        }
        else {
            EditorUtility.DisplayDialog("Campos obligatorios", " Usuario y contraseña son campos obligatorios", "Ok");
        }     
    }

    public void registrarse()
    {


    }

    public void abrirConfiguracion()
    {


    }
}
