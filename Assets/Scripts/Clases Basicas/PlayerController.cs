using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidBody; //Atributo que hara referencia al componente RigidBody2D de Unity que tiene el Jugador
    private Animator animator;
    public LayerMask zonaHierba;
    private float movimientoHorizontal;
    private float movimientoVertical;

    public Jugador Jugador { get; set; }

    // Start se llama al principio del juego(Al principio de la ejecucion de este script).Solo se ejecuta 1 vez
    async void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        await crearJugadorPrueba();
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

            if (true) {
                StartCoroutine(cargarEscenaCombatePokemonSalvaje());
            } 
        }
    }


    IEnumerator cargarEscenaCombatePokemonSalvaje()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        Utilidades.pausarMusicaEscenaActiva();//Se tiene que pausar la musica por que LoadSceneMode.Additive hace que la escena aunque se carga otra, se mantenga activa
        DontDestroyOnLoad(jugador);//this
        SceneManager.LoadScene("BattleWildPokemonScene", LoadSceneMode.Additive);
        yield return new WaitForSeconds(1);
        jugador.SetActive(false);  //this.gameObject.SetActive(false);
        
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


    private async Task crearJugadorPrueba() {
        try
        {
            ClsJugador b = new ClsJugador(1, "Usuario", "Constrasenha", "Correo", 5, 100, 200, new byte[0]);
            PokeAPI.Pokemon p1 = await APIListadosPokemonBL.obtenerPokemonDeApi(478);
            Pokemon pokemon1 = new Pokemon(p1);
            await pokemon1.obtenerTiposyDebilidades(p1.Types);

            PokeAPI.Pokemon p2 = await APIListadosPokemonBL.obtenerPokemonDeApi(257);
            Pokemon pokemon2 = new Pokemon(p2);
            await pokemon2.obtenerTiposyDebilidades(p2.Types);
            
            PokeAPI.Pokemon p3 = await APIListadosPokemonBL.obtenerPokemonDeApi(668);
            Pokemon pokemon3 = new Pokemon(p3);
            await pokemon3.obtenerTiposyDebilidades(p3.Types);

            PokeAPI.Pokemon p4 = await APIListadosPokemonBL.obtenerPokemonDeApi(184);
            Pokemon pokemon4 = new Pokemon(p4);
            await pokemon4.obtenerTiposyDebilidades(p4.Types);

            PokeAPI.Pokemon p5 = await APIListadosPokemonBL.obtenerPokemonDeApi(307);
            Pokemon pokemon5 = new Pokemon(p5);
            await pokemon5.obtenerTiposyDebilidades(p5.Types);

            List<PokemonJugador> equipoPokemon = new List<PokemonJugador>();
            equipoPokemon.Add(new PokemonJugador(pokemon1, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon2, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon3, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon4, 1, 1, 1, 100));
            equipoPokemon.Add(new PokemonJugador(pokemon5, 1, 1, 1, 100));

            List<ItemConCantidad> mochila = new List<ItemConCantidad>();
            mochila.Add(new ItemConCantidad(new Item(1, "Pocion", "Cura 20 hp", 0, 20, "POC"), 10));
            mochila.Add(new ItemConCantidad(new Item(2, "Pokeball", "Dispositivo para capturar pokemons", 0, 20, "POK"), 20));

            Jugador = new Jugador(b, equipoPokemon, mochila);
        }
        catch (Exception) {
            throw;
        }
        
    }


}
