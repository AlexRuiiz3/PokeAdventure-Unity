using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class RayCast : MonoBehaviour
{
    //Transform objeto;

    // Start is called before the first frame update
    void Start()
    {
       // objeto = GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x+5, transform.position.y+5));
        if (hit.collider != null) {
            if (hit.collider.CompareTag("Player")) {
                Debug.Log("Comienzo combate");
            }
        }
    }
}
