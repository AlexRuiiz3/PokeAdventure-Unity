using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidBody; //Atributo que hara referencia al componente RigidBody2D de Unity que tiene el Jugador
    private Animator animator;
    public LayerMask zonaHierba;
    private float movimientoHorizontal;
    private float movimientoVertical;

    public Jugador Jugador { get; }

    // Start se llama al principio del juego(Al principio de la ejecucion de este script).Solo se ejecuta 1 vez
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update se llama por cada frame(En los juegos se dice que van a 60 frame/s eso quiere decir que el metodo update se esta ejecutando 60 veces por segundo)
    void Update()
    {

        //Se obtiene el input del jugador(Teclado)
        movimientoHorizontal = Input.GetAxisRaw("Horizontal")*1.5f; //Metodo que devuelve 1 si pulsa la A o la D y -1 cualquier otra tecla
        movimientoVertical = Input.GetAxisRaw("Vertical")*1.5f; //Metodo que devuelve 1 si pulsa la W o la S y -1 cualquier otra tecla

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movimientoHorizontal *= 2.2f;
            movimientoVertical *= 2.2f;

        }

        if (movimientoHorizontal != 0 || movimientoVertical != 0)
        { //Si alguno de los no es igual a 0 significa que se esta moviento, si la X y la Y son 0 significa que el personaje esta parado
            //Por lo tanto como se sabe que el personaje se esta moviendo se hacen los set correspondiente de las amimaciones y se comprueba si se esta en una zona de hierba
            animator.SetFloat("moveX", movimientoHorizontal);
            animator.SetFloat("moveY", movimientoVertical);
            animator.SetBool("isMoving", true);
            comprobarZonaHierba();
        }
        else {
            animator.SetBool("isMoving", false);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    { //Es un metodo que tiene que actualizarce de manera mucho mas habitual que el Update(Este depende de a cuantos frame por segundos vayas), por eso aqui es donde se mete lo de la fisica

        //Vector representa un vector con dos posiciones X e Y en el mundo 
        rigidBody.velocity = new Vector2(movimientoHorizontal, movimientoVertical);

    }

    private void comprobarZonaHierba() {

        if (Physics2D.OverlapCircle(transform.position,0.2f,zonaHierba) != null) {

            if (Random.Range(1, 1000) <= 3) {

                Debug.Log($"Pokemon generado");
            } 
        }
    }
    /*
    //Metodo para cada vez que el jugador colisione con algo.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            switch (collision.gameObject.tag)
            {
                case "Trainer":

                    break;

                case "Item":

                    break;

                case "Cartel":
                    Debug.Log("Mensaje Cartel");
                    break;
            }
        }
    }*/
}
