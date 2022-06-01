using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionObjectoInteractable : MonoBehaviour
{
    public List<string> frases;
    private bool jugadorDentroRango;

    private void Update()
    {
        //TODO HACER METODO PARA DETERMINAR QUE AUDIO HAY QUE COGER 
        //Si el jugador esta dentro del rango, pulsa la tecla E y no hay un dialogo iniciado 
        if (jugadorDentroRango && Input.GetKey(KeyCode.E) && PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString())
        {
            UtilidadesEscena.activarPausarMusicaEscenaActiva(false);
            //UtilidadesEscena.activarMusicaIteraccionConAlgo("Get Item");
            PlayerPrefs.SetString("InteraccionConObjeto", gameObject.tag);//Se guarda el tipo de objeto con el que sea interactuado
            GameObject dialogo = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasDialogo");
            ControlDialogos controlDialogos = dialogo.transform.GetChild(0).gameObject.GetComponent<ControlDialogos>();
            if (gameObject.tag == "Trainer") { //Si se interactua con un entrenador
                TrainerNPC trainer = gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponentInChildren<TrainerNPC>();
                if (trainer.derrotado)
                {
                    controlDialogos.ListaFrases = trainer.FrasesDerrotado.ToArray();
                    PlayerPrefs.SetString("InteraccionConObjeto", "Trainer derrotado");
                }
                else {
                    UtilidadesEscena.activarMusicaIteraccionConAlgo("Get Item");
                    DatosGenerales.trainerLuchando = trainer;
                    controlDialogos.ListaFrases = trainer.Frases.ToArray();
                    StartCoroutine(trainer.activarExclamacionTrainerCombate());
                    GameObject jugador = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
                    DontDestroyOnLoad(jugador);
                }
            } else{
                controlDialogos.ListaFrases = frases.ToArray();
            }

            if (gameObject.tag == "Objeto") { //Si se trara de un objeto, se destruye ya que se suma al inventario del jugador
                Destroy(gameObject);
            }
            //Se muestra la interfaz del dialogo y se activa
            dialogo.SetActive(true);
            dialogo.transform.GetChild(0).gameObject.SetActive(true);//La imagen del dialogo
            controlDialogos.activarDialogo();
        }
    }

    //Metodo para controlar cuando el jugador entra dentro de la zona de iteracion que tiene asociado un objeto que es interactable
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger ) {
            jugadorDentroRango = true;
        }
    }
    //Metodo para controlar cuando el jugador sale de la zona de iteracion que tiene asociado un objeto que es interactable
    private void OnTriggerExit2D(Collider2D collision)
    {
        jugadorDentroRango = false;
    }

    /// <summary>
    /// Cabecera: IEnumerator activarExclamacionTrainerCombate()
    /// Comentario: Esta corrutina se encarga de activar la imagen de exclamacion que tiene un entrenador
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se activara la imagen de una exclacion duranto un breve periodo de tiempo y despues se desactivara
    /// </summary>
    /// <returns></returns>
    IEnumerator activarExclamacionTrainerCombate() {
        GameObject iconoExclamacion = gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;
        iconoExclamacion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        iconoExclamacion.SetActive(false);
       // Vector3 a = gameObject.transform.parent.gameObject.transform.position - GameObject.Find("Player").transform.position;
        //iconoExclamacion.transform.position = gameObject.transform.parent.gameObject.transform.position - GameObject.Find("Player").transform.position;

        gameObject.SetActive(false); //Se desactiva el gameObject que tiene el boxCollider, asi solo se podra combatir con el mismo entrenador solo una vez
        StopCoroutine(activarExclamacionTrainerCombate());
    }
}
