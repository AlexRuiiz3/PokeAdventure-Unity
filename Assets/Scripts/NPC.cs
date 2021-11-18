using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Rigidbody2D a;

    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.Mouse0)){
            Debug.Log("Mensaje NCP Activado");
        }
    }
}
