/*
 * Clase: Pokemon
 * 
 * Comentario: Esta clase representa a un pokemon con los atributos basicos de un pokemon.
 * 
 * Atributos:
 *          Basicos:
 *              id: Entero, Consultable, Modificable.
 *              nombre: Cadena, Consultable, Modificable.
 *              hp: Entero, Consultable, Modificable.
 *              hpMaximos: Entero, Consultable.
 *              nivel: Entero, Consultable, Modificable.
 *              ataque: Entero, Consultable, Modificable.
 *              defensa: Entero, Consultable, Modificable.
 *              velocidad: Entero, Consultable, Modificable.
 *              movimientos: Listado, Consultable, Modificable.
 *              tipos: Listado, Consultable, Modificable.
 *              debilidades: Listado, Consultable, Modificable.
 *          Derivados: Ninguno.
 *          Compartidos: Ninguno.
 *          
 * Metodos fundamentales(Propiedades)
 *              ID: public int ID { get; }
 *              Nombre: public string Nombre { get; set; }
 *              Hp: public int HP {get; set;}
 *              HpMaximos: public int HPMaximos {get; set;}
 *              Nivel: public int Nivel {get; set;}
 *              Ataque: public int Atque {get; set;}
 *              Defensa: public int Defensa {get; set;}
 *              Velocidad: public int Velocidad {get; set;}
 *              Movimientos: public int Movimientos {get; set;}
 *              Tipos: public int Tipos {get; set;}
 *              Debilidades: public int Debilidades {get; set;}
 * Metodos añadidos: 
 *                  public async Task obtenerDatosAsincronos(PokeAPI.Pokemon pokeAPI)
 *                  public bool recibirDanho(int danho).
 * 
 * Metodos heredados: Ninguno.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using PokeAPI;

public class Pokemon
{
    #region Atributos
    private int hp;
    private int nivel;
    #endregion

    #region Constructores
    //Contructor sin parametros
    public Pokemon()
    {
        ID = 0;
        Nombre = "";
        hp = 0;
        nivel = 0;
        Ataque = 0;
        Defensa = 0;
        Velocidad = 0;
        Movimientos = new List<MovimientoPokemon>();
        Tipos = new List<string>();
        Debilidades = new List<string>();
        HPMaximos = 0;
    }
    //Constructor con parametros
    public Pokemon(int id, string nombre, int hp,int hpMaximos, int nivel, int ataque, int defensa, int velocidad, List<MovimientoPokemon> movimientos, List<string> tipos, List<string> debilidades)
    {
        ID = id;
        Nombre = nombre;
        this.hp = hp;
        this.nivel = nivel;
        Ataque = ataque;
        Defensa = defensa;
        Velocidad = velocidad;
        Movimientos = movimientos;
        Tipos = tipos;
        Debilidades = debilidades;
        HPMaximos = hpMaximos;
    }
    //Constructor a partir de un PokeAPI.Pokemon 
    public Pokemon(PokeAPI.Pokemon pokemonApi)
    {
        ID = pokemonApi.ID;
        nivel = 1;
        Nombre = pokemonApi.Name.Split('-')[0];//Hay pokemons de la pokeApi que viene con una extension en el nombre, se elimina esa extension
        hp = (from pokemonStats in pokemonApi.Stats
              where pokemonStats.Stat.Name == "hp"
              select pokemonStats.BaseValue).First();
        Ataque = (from pokemonStats in pokemonApi.Stats
                  where pokemonStats.Stat.Name == "attack"
                  select pokemonStats.BaseValue).First();
        Defensa = (from pokemonStats in pokemonApi.Stats
                   where pokemonStats.Stat.Name == "defense"
                   select pokemonStats.BaseValue).First();
        Velocidad = (from pokemonStats in pokemonApi.Stats
                     where pokemonStats.Stat.Name == "speed"
                     select pokemonStats.BaseValue).First();
        HPMaximos = hp;
    }
    #endregion

    #region Metodos Fundamentales(Propiedades)
    public int ID { get; }
    public string Nombre { get; set; }
    public int HP
    {
        get { return hp; }
        set
        {
            if (value > HPMaximos) //Pokemon ps 150, se le ataca(20) y se queda con 130 si se usa una pocion que le cure 100 de esta manera se quedara en su maximo(150) y no con 230 cuando sus ps maximos eran 150
            {
                hp = HPMaximos;
            }
            else
            {
                if (value > 0)
                {
                    hp = value;
                }
                else
                {
                    hp = 0;
                }
            }
        }
    }
    //Quitar el set, al terminar el proyecto
    public int HPMaximos { get; set; } //Servira para tener guardado los ps maximo de un pokemon ya que cuando se cure a un pokemon hay que saber hasta donde curarlo.
    public int Nivel
    {
        get { return nivel; }
        set
        {
            if (value >= 1 && value <= 100)
            {
                nivel = value;
            }
        }
    }
    public int Ataque { get; set; }
    public int Defensa { get; set; }
    public int Velocidad { get; set; }
    public List<MovimientoPokemon> Movimientos { get; set; }
    public List<string> Tipos { get; set; }
    public List<string> Debilidades { get; set; }
    #endregion

    #region Metodos Añadidos 
    //Este metodo es necesario ya que en el constructor no puede a ver metodos async 
    public async Task<bool> obtenerDatosAsincronos(PokeAPI.Pokemon pokeAPI)
    {
        Tipos = await APIListadosPokemonBL.obtenerNombreTiposPokemon(pokeAPI.Types);
        Debilidades = await APIListadosPokemonBL.obtenerNombreTiposDebilesPokemon(pokeAPI.Types);
        Movimientos = await APIListadosPokemonBL.obtenerMovimientosAleatoriosPokemon(pokeAPI.Moves);

        return true;
    }
    /// <sumary>
    /// Cabecera: public bool recibirDanho(int danho) 
    /// Comentario: Este metodo se encarga de restar los hp de un pokemon en funcion del daño recibido.
    /// Entradas: int danho 
    /// Salidas: bool vivo
    /// Precondiciones: danho debera ser mayor que 0 sino al pokemon en vez de restarle vida, le incrementara.
    /// PostCondiciones: Se reducira el hp de un pokemon y se devolvera un bool cuyo valor puede ser:
    ///                  true: Si el hp del pokemon es superior a 0 despues de restarle el daño recibido.
    ///                  false: Si el hp del pokemon es inferior o igual a 0 despues de restarle el daño recibido.
    /// </summary>
    /// <param name="danho"></param>
    /// <returns>bool</returns>
    public bool recibirDanho(int danho)
    {
        bool vivo = true;
        hp -= danho;
        if (hp <= 0)
        {
            hp = 0;
            vivo = false;
        }
        return vivo;
    }
 
    public void establecerEstadisticasAlNivelActual(){
        if (nivel > 1)
        {
            HP += nivel * Random.Range(2, 5);
            HPMaximos = HP;
            Ataque += nivel * Random.Range(2, 5);
            Defensa += nivel * Random.Range(2, 5);
            Velocidad += nivel * Random.Range(2, 5);
        }
    }
    #endregion
}
