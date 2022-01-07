using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarPokemons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int numero = Random.Range(1,101); //Numero entre 1 y 100
        
        if (collision.CompareTag("Player") && numero >= 1) {
            Debug.Log("Pokemon Generado");
        }
    }
}
