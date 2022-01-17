using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionTrainerNPC : MonoBehaviour
{
    public GameObject player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger) {
            StartCoroutine(iniciarCombate());
        }
    }

    IEnumerator iniciarCombate() {
        GameObject iconoExclamacion = gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;
        iconoExclamacion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        iconoExclamacion.SetActive(false);
       // Vector3 a = gameObject.transform.parent.gameObject.transform.position - GameObject.Find("Player").transform.position;
        //iconoExclamacion.transform.position = gameObject.transform.parent.gameObject.transform.position - GameObject.Find("Player").transform.position;

        gameObject.SetActive(false); //Se desactiva el gameObject que tiene el boxCollider, asi solo se podra combatir con el mismo entrenador solo una vez
        Debug.Log("Iniciando batalla");
    }
}
