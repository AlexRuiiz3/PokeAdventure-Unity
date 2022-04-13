using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJugador : MonoBehaviour
{
    public GameObject menu;

    private void Start()
    {
    }

    private void Update()
    {
        if (PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString() && Input.GetKeyDown(KeyCode.C))
        {
            UtilidadesEscena.activarDesactivarMenuYTiempoJuego(menu);
        }
    }
    public void salir()
    {
        Time.timeScale = 1f;
        //EditorUtility.DisplayDialog("Datos incorrectos", "Vas a volver a la pantalla principal, los cambios no guardados se perderan", "Ok");
        SceneManager.LoadScene("MainScene");
    }
}
