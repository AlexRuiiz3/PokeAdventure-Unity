using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{

    private Rigidbody2D rigidBody; //Atributo que hara referencia al componente RigidBody2D de Unity que tiene el Jugador
    private Animator animator;
    private float movimientoHorizontal;
    private float movimientoVertical;


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
        movimientoHorizontal = Input.GetAxisRaw("Horizontal"); //Metodo que devuelve 1 si pulsa la A o la D y -1 cualquier otra tecla;
        movimientoVertical = Input.GetAxisRaw("Vertical"); //Metodo que devuelve 1 si pulsa la W o la S y -1 cualquier otra tecla
        ModificarVariablesAnimaciones();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            movimientoHorizontal *= (float)1.8;
            movimientoVertical *= (float)1.8;
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

    /// <summary>
    /// Cabecera: private void ModificarVariablesAnimaciones()
    /// Comentario: Este metodo se encarga de modificar las variables booleanas de un Animator
    ///             dependiendo de que tecla se pulse
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se modificaran las variables booleanas de un objeto Animator que determinan que 
    ///                  animacion se activa en funcion de la tecla que se pulse, el resultado de estas puede ser:
    ///                  -Todas false
    ///                  -Una true y el resto false
    /// </summary>
    private void ModificarVariablesAnimaciones() {

        animator.SetBool("movimientoIzquierda", false);
        animator.SetBool("movimientoDerecha", false);
        animator.SetBool("movimientoArriba", false);
        animator.SetBool("movimientoAbajo", false);

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("movimientoIzquierda", true);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("movimientoDerecha", true);

        }
        else if (Input.GetKey(KeyCode.W))
        {

            animator.SetBool("movimientoArriba", true);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("movimientoAbajo", true);

        }
    }
}
