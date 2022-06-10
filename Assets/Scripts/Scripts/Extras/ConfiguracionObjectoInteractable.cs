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

        //Si el jugador esta dentro del rango, pulsa la tecla E y no hay un dialogo iniciado  y no hay abierto un menu de iteracion
        if (jugadorDentroRango && Input.GetKey(KeyCode.E) && PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString() && PlayerPrefs.GetInt("MenuIteracionAbierto") == 0)
        {
            PlayerPrefs.SetString("InteraccionConObjeto", gameObject.tag);//Se guarda el tipo de objeto con el que sea interactuado
            GameObject dialogo = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasDialogo");
            ControlDialogos controlDialogos = dialogo.transform.GetChild(0).gameObject.GetComponent<ControlDialogos>();
            if (gameObject.tag == "TrainerInteraccion") { //Si se interactua con un entrenador
                TrainerNPC trainer = gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponentInChildren<TrainerNPC>();
                trainer.Imagen = (from spriteNPC in Resources.LoadAll<Sprite>("Imagenes/Trainers/" + transform.parent.gameObject.name)
                                 where spriteNPC.name == "Abajo"
                                 select spriteNPC).First();
                if (trainer.derrotado)
                {
                    controlDialogos.ListaFrases = trainer.FrasesDerrotado.ToArray();
                    PlayerPrefs.SetString("InteraccionConObjeto", "Trainer derrotado");
                }
                else {
                    UtilidadesEscena.activarPausarMusicaEscenaActiva(false);
                    UtilidadesEscena.activarMusicaTemporal($"Batalla/TrainerSeesYou{Random.Range(1, 5)}", true);
                    DatosGenerales.trainerLuchando = trainer;
                    controlDialogos.ListaFrases = trainer.Frases.ToArray();
                    StartCoroutine(trainer.activarExclamacionTrainerCombate());
                    GameObject jugador = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("Player"));
                    DontDestroyOnLoad(jugador);
                }
            } else{
                determinarAccionSegunObjecto(gameObject.tag);
                controlDialogos.ListaFrases = frases.ToArray();
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

        gameObject.SetActive(false); //Se desactiva el gameObject que tiene el boxCollider, asi solo se podra combatir con el mismo entrenador solo una vez
        StopCoroutine(activarExclamacionTrainerCombate());
    }

    private void determinarAccionSegunObjecto(string tag) {
        switch (tag)
        {
            case "PC": UtilidadesEscena.llamarActivarAudioMomentaneo("Iteracion/OpenPC",1.5f); break;
            case "Item":
                UtilidadesEscena.activarPausarMusicaEscenaActiva(false);
                UtilidadesEscena.activarMusicaTemporal("Iteracion/GetItem", false);
                Destroy(gameObject); 
                break;
        }
    }
}
