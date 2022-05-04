using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    public InputField inputNombreUsuario;
    public InputField inputContrasenha;
    public TMP_InputField inputNombreUsuarioRegistro;
    public TMP_InputField inputContrasenhaRegistro;
    public TMP_InputField inputContrasenhaRepetida;
    public TMP_InputField inputCorreoElectronico;

    private void Start()
    {
        PlayerPrefs.SetString("GameLanguage", "es");
        if (PlayerPrefs.GetInt("BaseDatosCreada") == 0) { 
         ConfiguracionDB.createDB();
        }
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
                    UtilidadesEscena.mostrarMensajeError("El usuario o la contrasena no son validos");
                }
            }
            catch (Exception) {
                UtilidadesEscena.mostrarMensajeError("Ocurrio un error al intentar acceder a la base de datos");
            }
        }
        else {
            UtilidadesEscena.mostrarMensajeError("El usuario y la contrasena son campos obligatorios");
        }     
    }

    public void registrarUsuario()
    {

        bool existeNombreUsuario;
        if (!Utilidades.comprobarCadenaVacia(inputNombreUsuarioRegistro.text)) //Si el nombre de usuario no esta vacio o es null
        {
            existeNombreUsuario = UtilidadesDal.comprobarSiExisteNombreUsuario(inputNombreUsuarioRegistro.text);
            if (!existeNombreUsuario) //Si no existe ese nombre de usuario
            {
                if (!Utilidades.comprobarCadenaVacia(inputContrasenhaRegistro.text)) //Si la contraseña no esta vacia
                {
                    if (inputContrasenhaRegistro.text.Equals(inputContrasenhaRepetida.text)) //Si las contraseñas coinciden
                    {
                        if (!Utilidades.comprobarCadenaVacia(inputCorreoElectronico.text)) //Si el correo electronico esta vacio 
                        {
                            if (inputCorreoElectronico.text.EndsWith("@gmail.com") || inputCorreoElectronico.text.EndsWith("@gmail.es")) //Si el correo electronico termina por @gmail.com o @gmail.es
                            {
                                GestoraJugadorBL.insertarJugador(new ClsJugador(0, inputNombreUsuarioRegistro.text, inputContrasenhaRegistro.text, inputCorreoElectronico.text, 0, new byte[0]));
                                Debug.Log("Usuario registrado");
                            }
                            else
                            {
                                UtilidadesEscena.mostrarMensajeError("Correo electronico mal escrito. Debe acabar en @gmail.com o @gmail.es");
                            }
                        }
                        else
                        {
                            UtilidadesEscena.mostrarMensajeError("El correo electronico no puede estar vacio");
                        }
                    }
                    else
                    {
                        //Las contraseñas no coinciden 
                        UtilidadesEscena.mostrarMensajeError("Las contrasenas no coinciden");
                    }
                }
                else
                {
                    //La contraseña esta vacia
                    UtilidadesEscena.mostrarMensajeError("La contrasena no puede estar vacia");
                }
            }
            else
            {
                //NombreUsuario ya existe
                UtilidadesEscena.mostrarMensajeError("El nombre de usuario ya existe");
            }
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("El nombre de usuario no puede estar vacio");
        }
    }

}
