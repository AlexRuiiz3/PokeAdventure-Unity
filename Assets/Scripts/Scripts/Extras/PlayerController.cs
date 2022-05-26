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

    private Rigidbody2D rigidBody; //Atributo que hara referencia al componente RigidBody2D de Unity que tiene el Jugador
    private Animator animator;
    private float movimientoHorizontal;
    private float movimientoVertical;

    public LayerMask zonaHierba; //Atributo que servira para poder detectar cuando un jugador este por una zona de hierba
    public Jugador Jugador { get; set; }

    // Start se llama al principio del juego(Al principio de la ejecucion de este script).Solo se ejecuta 1 vez
    void Start()
    {
        PlayerPrefs.SetString("EstadoDialogo", DialogEstate.END.ToString());// Quitar
        PlayerPrefs.SetString("GameLanguage", "es"); //Quitar de aqui es solo prueba porque esto va en em MenuPrincipal 
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        obtenerDatosJugador("a","a");//Eliminar de aqui iria en la de inicio y tmb en la de registro
        Jugador.EquipoPokemon[0].HPMaximos = 100;
        Jugador.EquipoPokemon[0].HP = 49;
        Jugador.EquipoPokemon[1].HPMaximos = 100;
        Jugador.EquipoPokemon[1].HP = 1;
        if (!DatosGuardarJugador.ListasObtenidas) {
            DatosGuardarJugador.PokemonsEncontradosJugador = ListadosPokemonEncontradosJugadorBL.obtenerPokemonsEncontradosDeJugador(Jugador.ID);//Eliminar de aqui
            DatosGuardarJugador.PokemonsAlmacenadosPC = ListadosPokemonsJugadorBL.obtenerPokemonsNoEquipadosJugador(Jugador.ID);
            DatosGuardarJugador.ListasObtenidas = true;
        }
        DatosGuardarJugador.PokemonsAlmacenadosPC.Add(Jugador.EquipoPokemon[2]);
    }

    // Update se llama por cada frame(En los juegos se dice que van a 60 frame/s eso quiere decir que el metodo update se esta ejecutando 60 veces por segundo)
    void Update()
    {
        if (PlayerPrefs.GetString("EstadoDialogo") == DialogEstate.END.ToString())//Para evitar que cuando el usuario este en interactuando con un objeto o NPC que tiene un dialogo, se pueda mover
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

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    { //Es un metodo que tiene que actualizarce de manera mucho mas habitual que el Update(Este depende de a cuantos frame por segundos vayas), por eso aqui es donde se mete lo de la fisica
        //Vector representa un vector con dos posiciones X e Y en el mundo 
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
                UtilidadesEscena.pausarMusicaEscenaActiva();//Se tiene que pausar la musica por que LoadSceneMode.Additive hace que la escena aunque se carga otra, se mantenga activa
                DontDestroyOnLoad(gameObject);
                SceneManager.LoadScene("BattleWildPokemonScene", LoadSceneMode.Additive);
                //yield return new WaitForSeconds(1);
                gameObject.SetActive(false);
                //StartCoroutine(cargarEscenaCombatePokemonSalvaje());
            }
        }
    }


    private void obtenerDatosJugador(string nombreUsuario, string contrasenha)//No va aqui va cuando se inicia sesion y despues de resgistrase
    {
        try
        {
            ClsJugador jugadorBase = ListadosJugadorBL.obtenerJugador(nombreUsuario, contrasenha);
            List<PokemonJugador> pokemonsJugador = ListadosPokemonsJugadorBL.obtenerPokemonsJugadorEquipados(jugadorBase.ID);
            List<ItemConCantidad> itemsJugador = ListadosItemBL.obtenerItemsJugador(jugadorBase.ID);

            Jugador = new Jugador(jugadorBase,pokemonsJugador,itemsJugador);
            Jugador.Dinero = 2000;// De prueba, eliminar de aqui 
        }
        catch (Exception)
        {
        }
    }
    //Metodo 
    public void iniciarCoroutineAsignarObjetoEncontrado() {
        StartCoroutine(asignarObjetoEncontrado());
    }
    
    IEnumerator asignarObjetoEncontrado() //Deberia de ir en ultilidadesObjetoInteractable, pero al ser una corrutina debe de ir en un script que este asociado a un gameObject del juego
    {
        GameObject canvasObjetoRecogigo = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasObjetoRecogido");
        int cantidadItem = (int)UnityEngine.Random.Range(1f, 3f);
        //Poner try catch
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
    }
}
