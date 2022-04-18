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

    private void Start()
    {
        PlayerPrefs.SetString("GameLanguage", "es");
        ConfiguracionDB.createDB();
    }

    public void iniciarSesion() {
        string nombreUsuario = inputNombreUsuario.text,
               contrasenha = inputContrasenha.text;

        if (!string.IsNullOrEmpty(nombreUsuario.Trim()) && !string.IsNullOrEmpty(contrasenha.Trim()))
        {
            try {
                if (ListadosJugadorBL.comprobarExistenciaNombreUsuarioContrasenha(nombreUsuario,contrasenha)) 
                {

                    SceneManager.LoadScene("LobbyScene");
                }
                else
                {
                    EditorUtility.DisplayDialog("Datos incorrectos", "Usuario o contraseña no validos", "Ok");
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

}
