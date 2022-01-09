using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OpcionRegistrarse : MonoBehaviour
{
    public TMP_InputField inputNombreUsuario; //TMP_Text es tipo de text que tiene inputFieldl-TextMeshPro
    public TMP_InputField inputContrasenha;
    public TMP_InputField inputContrasenhaRepetida;
    public TMP_InputField inputCorreoElectronico;

    void Start()
    {
        inputNombreUsuario.text = "";
        gameObject.SetActive(false);
    }

    public void registrarUsuario() {

        bool existeNombreUsuario;
        if (!Utilidades.comprobarCadenaVacia(inputNombreUsuario.text)) //Si el nombre de usuario no esta vacio o es null
        {
            existeNombreUsuario = UtilidadesDal.comprobarSiExisteNombreUsuario(inputNombreUsuario.text);
            if (!existeNombreUsuario) //Si no existe ese nombre de usuario
            {
                if (!Utilidades.comprobarCadenaVacia(inputContrasenha.text)) //Si la contraseña no esta vacia
                {  
                    if (inputContrasenha.text.Equals(inputContrasenhaRepetida.text)) //Si las contraseñas coinciden
                    {
                        if (!Utilidades.comprobarCadenaVacia(inputCorreoElectronico.text)) //Si el correo electronico esta vacio
                        {
                            if (inputCorreoElectronico.text.EndsWith("@gmail.com") || inputCorreoElectronico.text.EndsWith("@gmail.es")) //Si el correo electronico termina por @gmail.com o @gmail.es
                            {
                                GestoraJugador.insertarJugador(new ClsJugador(inputNombreUsuario.text, inputContrasenha.text, inputCorreoElectronico.text, 1, 0, new byte[0]));
                                EditorUtility.DisplayDialog("Campos obligatorios", "guardado con exito", "Ok");
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Campos obligatorios", " correo mal escrito ", "Ok");
                            }
                        }
                        else {
                            EditorUtility.DisplayDialog("Campos obligatorios", " correo vacio ", "Ok");
                        }
                    }
                    else {
                        //Las contraseñas no coinciden 
                        EditorUtility.DisplayDialog("Campos obligatorios", " contraseñas no coinciden ", "Ok");
                    }
                }
                else {
                    //La contraseña esta vacia
                    EditorUtility.DisplayDialog("Campos obligatorios", " contraseñas vacia", "Ok");
                }
            }
            else {
                //NombreUsuario ya existe
                EditorUtility.DisplayDialog("Campos obligatorios", " NombreUsuario ya existe", "Ok");
            }
        }
        else {
            //NombreUsuario esta vacio
            EditorUtility.DisplayDialog("Campos obligatorios", " NombreUsuario esta vacio", "Ok");
        }
    }
}
