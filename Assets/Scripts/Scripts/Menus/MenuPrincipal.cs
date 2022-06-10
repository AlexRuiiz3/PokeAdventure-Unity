using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        resetearPlayerPrefs();
        Time.timeScale = 1f;
        GameObject jugador = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
        DontDestroyOnLoad(jugador);

        if (PlayerPrefs.GetInt("BaseDatosCreada") == 0)
        {
            ConfiguracionDB.createDB();
        }
    }
    /// <summary>
    /// Cabecera: public void iniciarSesion()
    /// Comentario: Este metodo se encargar de obtener el nombre de usuario y contraseña de los campos, y comprobar si son validos para dar acceso a un jugador.  
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se concedera acceso al usuario si los el valor de los campos nombre de usuario y contraseña son validos(Estan registrados).
    /// </summary>
    public void iniciarSesion()
    {
        string nombreUsuario = inputNombreUsuario.text,
               contrasenha = inputContrasenha.text;

        if (!string.IsNullOrEmpty(nombreUsuario.Trim()) && !string.IsNullOrEmpty(contrasenha.Trim()))
        {
            try
            {
                if (ListadosJugadorBL.comprobarExistenciaNombreUsuarioContrasenha(nombreUsuario, contrasenha))
                {

                    if (ListadosPokemonsJugadorBL.obtenerNumeroPokemonsJugador(nombreUsuario, contrasenha) < 1)
                    {
                        PlayerPrefs.SetString("NombreUsuarioIniciado", nombreUsuario);
                        PlayerPrefs.SetString("ContrasenhaUsuarioIniciado", contrasenha);
                        UtilidadesEscena.precargarEscena("GetFirstPokemonScene");
                    }
                    else
                    {
                        Utilidades.obtenerDatosJugador(nombreUsuario, contrasenha);
                        UtilidadesEscena.precargarEscena("LobbyScene");
                    }
                }
                else
                {
                    UtilidadesEscena.mostrarMensajeError("El usuario o la contrasena no son validos");
                }
            }
            catch (Exception)
            {
                UtilidadesEscena.mostrarMensajeError("Ocurrio un error al intentar acceder a la base de datos");
            }
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("El usuario y la contrasena son campos obligatorios");
        }
    }

    /// <summary>
    /// Cabecera: public void registrarUsuario()
    /// Comentario: Este metodo se encarga de obtener los datos necesarios para crear una cuenta a un jugador, si algun dato no es valido la cuenta del jugador no se creara y se informara al usuario del motivo por el que no se pudo crear la cuenta.
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se registrara en la base datos un usuario. Si alguna informacion no es valida el usuario no se registrara en la base de datos y se informara al jugador de ello. 
    /// </summary>
    public void registrarUsuario()
    {
        bool existeNombreUsuario;
        if (!Utilidades.comprobarCadenaVacia(inputNombreUsuarioRegistro.text)) //Si el nombre de usuario no esta vacio o es null
        {
            try
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
                                    try
                                    {
                                        PlayerPrefs.SetString("NombreUsuarioIniciado", inputNombreUsuarioRegistro.text);
                                        PlayerPrefs.SetString("ContrasenhaUsuarioIniciado", inputContrasenhaRegistro.text);
                                        GestoraJugadorBL.insertarJugador(new ClsJugador(0, inputNombreUsuarioRegistro.text, inputContrasenhaRegistro.text, inputCorreoElectronico.text, 250, new byte[0]));
                                        UtilidadesEscena.precargarEscena("GetFirstPokemonScene");
                                    }
                                    catch (Exception)
                                    {
                                        UtilidadesEscena.mostrarMensajeError("Error durante el guardado de los datos del jugador");
                                    }

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
                            UtilidadesEscena.mostrarMensajeError("Las contrasenas no coinciden");
                        }
                    }
                    else
                    {
                        UtilidadesEscena.mostrarMensajeError("La contrasena no puede estar vacia");
                    }
                }
                else
                {
                    UtilidadesEscena.mostrarMensajeError("El nombre de usuario ya existe");
                }
            } catch (Exception) {
                UtilidadesEscena.mostrarMensajeError("Error con los datos de la base de datos");
            }
        }
        else
        {
            UtilidadesEscena.mostrarMensajeError("El nombre de usuario no puede estar vacio");
        }
    }
    private void resetearPlayerPrefs() {
        PlayerPrefs.SetString("NameLastScene", "");
        PlayerPrefs.SetString("GameLanguage", "es");
        PlayerPrefs.SetInt("MenuIteracionAbierto", 0);
        PlayerPrefs.SetString("InteraccionConObjeto", "");
        PlayerPrefs.SetString("EstadoDialogo", DialogEstate.END.ToString());
    }
}
