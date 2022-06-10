using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody; 
    private Animator animator;
    private float movimientoHorizontal;
    private float movimientoVertical;

    public LayerMask zonaHierba; //Atributo que servira para poder detectar cuando un jugador este por una zona de hierba
    public Jugador Jugador { get; set; }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString() && Time.timeScale == 1)//Para evitar que cuando el usuario este en interactuando con un objeto o NPC que tiene un dialogo, se pueda mover
        {
            //Se obtiene el input del jugador(Teclado)
            movimientoHorizontal = Input.GetAxisRaw("Horizontal") * 1.9f; //Metodo que devuelve 1 si pulsa la A o la D y -1 cualquier otra tecla
            movimientoVertical = Input.GetAxisRaw("Vertical") * 1.9f; //Metodo que devuelve 1 si pulsa la W o la S y -1 cualquier otra tecla

            if (Input.GetKey(KeyCode.LeftShift)) //Para que el jugador vaya mas rapido(Correr)
            {
                movimientoHorizontal *= 2.15f;
                movimientoVertical *= 2.15f;

            }

            if (movimientoHorizontal != 0 || movimientoVertical != 0)
            { //Si alguno de los no es igual a 0 significa que se esta moviento, si la X y la Y son 0 significa que el personaje esta parado
              //Por lo tanto como se sabe que el personaje se esta moviendo se hacen los set correspondiente de las amimaciones y se comprueba si se esta en una zona de hierba
                animator.SetFloat("moveX", movimientoHorizontal);
                animator.SetFloat("moveY", movimientoVertical);
                animator.SetBool("isMoving", true);
                comprobarZonaHierba();
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private void FixedUpdate()
    { //Es un metodo que tiene que actualizarce de manera mucho mas habitual que el Update(Este depende de a cuantos frame por segundos vayas), por eso aqui es donde se mete lo de la fisica 
        rigidBody.velocity = new Vector2(movimientoHorizontal, movimientoVertical);
    }

    //Getters y setters
        //MovimientoHorizontal
        public float getMovimientoHorizontal() => movimientoHorizontal;
        public void setMovimientoHorizontal(float movimientoHorizontal) { this.movimientoHorizontal = movimientoHorizontal; }

        //MovimientoVertical
        public float getMovimientoVertical() => movimientoVertical;
        public void setMovimientoVertical(float movimientoVertical) { this.movimientoVertical = movimientoVertical; }

    //Metodos añadidos
    private void comprobarZonaHierba()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, zonaHierba) != null)
        {
            if (UnityEngine.Random.Range(1, 851) <= 2)
            {
                UtilidadesEscena.activarPausarMusicaEscenaActiva(false);//Se tiene que pausar la musica por que LoadSceneMode.Additive hace que la escena aunque se carga otra, se mantenga activa
                PlayerPrefs.SetString("NameNextScene", "BattleWildPokemonScene");
                SceneManager.LoadScene("BattleWildPokemonScene",LoadSceneMode.Additive);
            }
        }
    }

    //Metodo para poder comenzar la corrutina desde el script ultilidadesObjetoInteractable
    public void iniciarCoroutineAsignarObjetoEncontrado() { //Necesario ya que desde donde se llama no se puede llamar a StartCoroutine
        StartCoroutine(asignarObjetoEncontrado());
    }
    
    /// <summary>
    /// Cabecera: public IEnumerator asignarObjetoEncontrado()
    /// Comentario: Esta corrutina se encarga de asignar al jugador un objecto que recoge. 
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se mostrara en pantalla un menu temporal con el objeto recogido, y se asignara a la mochila del jugador el objeto recogido.
    /// </summary>
    public IEnumerator asignarObjetoEncontrado() //Deberia de ir en ultilidadesObjetoInteractable, pero al ser una corrutina debe de ir en un script que este asociado a un gameObject del juego
    {
        GameObject canvasObjetoRecogigo = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasObjetoRecogido");
        int cantidadItem = (int)UnityEngine.Random.Range(1f, 3f);
        try{
        ItemConCantidad itemRecogido = new ItemConCantidad(ListadosItemBL.obtenerItemAleatorio(), cantidadItem);
        Sprite iconoItem = Resources.LoadAll<Sprite>("Imagenes/Items/").First(g => g.name == itemRecogido.Nombre);

        //GetChild(0) hace referencia a la imagen de fondo
        canvasObjetoRecogigo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = iconoItem;
        canvasObjetoRecogigo.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"+{cantidadItem}";
        canvasObjetoRecogigo.gameObject.SetActive(true);
        if (Jugador.Mochila.Contains(itemRecogido))
        {
            Jugador.Mochila.Find(g => g.ID == itemRecogido.ID).Cantidad += itemRecogido.Cantidad;
        }
        else
        {
            Jugador.Mochila.Add(itemRecogido);
        }
        yield return new WaitForSeconds(1.2f);
        canvasObjetoRecogigo.gameObject.SetActive(false);
        StopCoroutine(asignarObjetoEncontrado());
        }catch(Exception){
            UtilidadesEscena.mostrarMensajeError("Error en la generacion del item");
            StopCoroutine(asignarObjetoEncontrado());
        }
    }
}
