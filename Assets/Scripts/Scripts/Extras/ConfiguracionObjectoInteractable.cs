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
        if (jugadorDentroRango && Input.GetKey(KeyCode.E) && PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString())
        {
            PlayerPrefs.SetString("InteraccionConObjeto", gameObject.tag);
            GameObject dialogo = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasDialogo");
            ControlDialogos controlDialogos = dialogo.transform.GetChild(0).gameObject.GetComponent<ControlDialogos>();
            if (gameObject.tag == "Trainer") {
                TrainerNPC trainer = gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponentInChildren<TrainerNPC>();
                if (trainer.derrotado)
                {
                    controlDialogos.ListaFrases = trainer.FrasesDerrotado.ToArray();
                    PlayerPrefs.SetString("InteraccionConObjeto", "Trainer derrotado");
                }
                else {
                    controlDialogos.ListaFrases = trainer.Frases.ToArray();
                    StartCoroutine(trainer.activarExclamacionTrainerCombate());
                }
            } else{
                controlDialogos.ListaFrases = frases.ToArray();
            }

            if (gameObject.tag == "Objeto") {
                Destroy(gameObject);
            }
            dialogo.SetActive(true);
            dialogo.transform.GetChild(0).gameObject.SetActive(true);//La imagen del dialogo
            controlDialogos.activarDialogo();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger ) {
            jugadorDentroRango = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        jugadorDentroRango = false;
    }

    public IEnumerator activarExclamacionTrainerCombate() {
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
